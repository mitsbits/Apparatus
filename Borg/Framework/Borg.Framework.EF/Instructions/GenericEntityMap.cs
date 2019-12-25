using Borg.Framework.EF.Instructions.Attributes.Schema;
using Borg.Framework.EF.Instructions.Contracts;
using Borg.Platform.EF.Instructions.Contracts;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Borg.Framework.EF.Instructions
{
    public abstract partial class GenericEntityMap<TEntity, TDbContext> : EntityMapBase, IEntityMap<TEntity, TDbContext>, IEntityTypeConfiguration<TEntity>, IDbTypeConfiguration where TEntity : class where TDbContext : DbContext
    {
        protected bool ProcessAnnotations = true;

        protected GenericEntityMap() : base(typeof(TEntity), typeof(TDbContext))
        {
        }

        protected string GetSequenceName(string property)
        {
            var type = typeof(TEntity);
            var propInfo = type.GetProperty(property);
            if (propInfo == null)
            {
                throw new InvalidPropertyName(type, property);
            }
            return $"{typeof(TEntity).Name}_{property}_seq";
        }

        #region OnModelCreating

        public override void ConfigureDb(ModelBuilder builder)
        {
            SetFieldDefaults(builder);
            if (!ProcessAnnotations) return;
            SequenceDefinition(builder);
            IndexDefinition(builder);
            StringFieldsLenth(builder);
            PrincipalForeignKeyDefinition(builder);
            //HasManyDefinition(builder);
        }

        public virtual void ConfigureEntity(EntityTypeBuilder<TEntity> builder)
        {
            throw new NotImplementedException();
        }

        void IEntityTypeConfiguration<TEntity>.Configure(EntityTypeBuilder<TEntity> builder)
        {
            ConfigureEntity(builder);
        }

        private void SetFieldDefaults(ModelBuilder builder)
        {
            var collection = typeof(TEntity).GetProperties().Where(x => x.PropertyType.Equals(typeof(string)));
            foreach (var prop in collection)
            {
                builder.Entity<TEntity>().Property(prop.Name).HasMaxLength(100).IsUnicode(true).HasDefaultValue("");
            }
        }

        private void PrincipalForeignKeyDefinition(ModelBuilder builder)
        {
            var source = typeof(TEntity).GetProperties();
            foreach (var p in source)
            {
                var attr = p.GetCustomAttribute<PrincipalForeignKeyDefinitionAttribute>();
                if (attr != null)
                {
                    var principalType = p.PropertyType;
                    builder.Entity(principalType).HasMany(EntityType).WithOne(p.Name).HasForeignKey(attr.Columns).OnDelete(DeleteBehavior.ClientSetNull);
                }
            }
        }

        private void StringFieldsLenth(ModelBuilder builder)
        {
            var source = typeof(TEntity).GetProperties();
            foreach (var p in source)
            {
                if (p.PropertyType.Equals(typeof(string)))
                {
                    var mla = p.GetCustomAttribute<MaxLengthAttribute>();
                    var sla = p.GetCustomAttribute<StringLengthAttribute>();
                    if (mla == null && sla == null)
                    {
                        builder.Entity<TEntity>().Property(p.Name).HasMaxLength(100);
                    }

                    var uniAttr = p.GetCustomAttribute<UnicodeAttribute>();
                    if (uniAttr != null)
                    {
                        builder.Entity<TEntity>().Property(p.Name).IsUnicode(uniAttr.IsUnicode);
                    }
                    else
                    {
                        builder.Entity<TEntity>().Property(p.Name).IsUnicode(true);
                    }
                }
            }
        }

        private void SequenceDefinition(ModelBuilder builder)
        {
            var source = typeof(TEntity).GetProperties();
            foreach (var p in source)
            {
                foreach (var a in p.GetCustomAttributes<SequenceDefinitionAttribute>(true))
                {
                    var sqa = a as SequenceDefinitionAttribute;
                    if (sqa != null)
                    {
                        var sequenceName = $"{typeof(TEntity).Name}_{p.Name}_seq";
                        var options = ScriptOptions.Default.AddReferences(EntityType.Assembly);
                        var keyExpr = $"x => x.{p.Name}";
                        var keyExpression = AsyncHelpers.RunSync(() => CSharpScript.EvaluateAsync<Expression<Func<TEntity, object>>>(keyExpr, options));

                        builder.HasSequence<int>(sequenceName)
                               .StartsAt(sqa.StartsAt)
                               .IncrementsBy(sqa.IncrementsBy);

                        builder.Entity<TEntity>().Property(keyExpression).HasDefaultValueSql($"NEXT VALUE FOR {sequenceName}");

                        var ixsqa = sqa as IndexSequenceDefinitionAttribute;
                        if (ixsqa != null)
                        {
                            var pksqa = ixsqa as PrimaryKeySequenceDefinitionAttribute;
                            if (pksqa != null)
                            {
                                builder.Entity<TEntity>().HasKey(keyExpression).HasName($"PK_{EntityType.Name}_{p.Name}").IsClustered();
                            }
                            else
                            {
                                builder.Entity<TEntity>().HasIndex(keyExpression).HasName($"IX_{EntityType.Name}_{p.Name}");
                            }
                        }
                    }
                }
            }
        }

        private void IndexDefinition(ModelBuilder builder)
        {
            var props = EntityType.GetProperties().Where(x => x.GetCustomAttribute<IndexDefinitionAttribute>() != null);
            if (!props.Any()) return;
            var options = ScriptOptions.Default.AddReferences(EntityType.Assembly);
            var dataset = props.Select(x => new { prop = x, attr = x.GetCustomAttribute<IndexDefinitionAttribute>() });
            var groups = dataset.GroupBy(x => x.attr.IndexName);

            string IndexName(string indexName, IndexDefinitionAttribute.IndexDefinitionMode mode, IEnumerable<PropertyInfo> properties)
            {
                if (indexName != IndexDefinitionAttribute.DefaultIndexName) return indexName;

                var propertypath = string.Join("_", properties.Select(x => x.Name));
                var prefix = string.Empty;
                switch (mode)
                {
                    case IndexDefinitionAttribute.IndexDefinitionMode.UniqueIndex:
                        prefix = $"UX_{EntityType.Name}";
                        break;

                    case IndexDefinitionAttribute.IndexDefinitionMode.PrimaryKey:
                        prefix = $"PK_{EntityType.Name}";
                        break;

                    default:
                        prefix = $"IX_{EntityType.Name}";
                        break;
                }
                return $"{prefix}_{propertypath}";
            }

            foreach (var @group in groups)
            {
                var indexName = group.Key;
                if (@group.Count() == 1)
                {
                    var prop = props.First();
                    var keyExpr = $"x => x.{prop.Name}";
                    Expression<Func<TEntity, object>> keyExpression = AsyncHelpers.RunSync(() => CSharpScript.EvaluateAsync<Expression<Func<TEntity, object>>>(keyExpr, options));

                    if (prop.GetCustomAttribute<UniqueIndexDefinitionAttribute>() != null)
                    {
                        if (prop.GetCustomAttribute<PrimaryKeyDefinitionAttribute>() != null)
                        {
                            indexName = IndexName(indexName, IndexDefinitionAttribute.IndexDefinitionMode.PrimaryKey, group.Select(x => x.prop));
                            builder.Entity<TEntity>().HasKey(keyExpression).HasName(indexName).IsClustered();
                        }
                        else
                        {
                            indexName = IndexName(indexName, IndexDefinitionAttribute.IndexDefinitionMode.UniqueIndex, group.Select(x => x.prop));
                            builder.Entity<TEntity>().HasIndex(keyExpression).IsUnique().HasName(indexName);
                        }
                    }
                    else
                    {
                        indexName = IndexName(indexName, IndexDefinitionAttribute.IndexDefinitionMode.Index, group.Select(x => x.prop));
                        builder.Entity<TEntity>().HasIndex(keyExpression).HasName(indexName);
                    }
                }
                else
                {
                    var sb = new StringBuilder("x => new { ");
                    sb.Append(string.Join(", ", @group.OrderBy(x => x.attr.Order).Select(x => $"x.{x.prop.Name}")));
                    sb.Append(" }");
                    var keyExpr = sb.ToString();
                    Expression<Func<TEntity, object>> keyExpression = AsyncHelpers.RunSync(() => CSharpScript.EvaluateAsync<Expression<Func<TEntity, object>>>(keyExpr, options));
                    if (@group.All(x => x.prop.GetCustomAttribute<UniqueIndexDefinitionAttribute>() != null))
                    {
                        if (@group.All(x => x.prop.GetCustomAttribute<PrimaryKeyDefinitionAttribute>() != null))
                        {
                            indexName = IndexName(indexName, IndexDefinitionAttribute.IndexDefinitionMode.PrimaryKey, group.Select(x => x.prop));
                            builder.Entity<TEntity>().HasKey(keyExpression).HasName(indexName).IsClustered();
                        }
                        else
                        {
                            indexName = IndexName(indexName, IndexDefinitionAttribute.IndexDefinitionMode.UniqueIndex, group.Select(x => x.prop));
                            builder.Entity<TEntity>().HasIndex(keyExpression).IsUnique().HasName(indexName);
                        }
                    }
                    else
                    {
                        indexName = IndexName(indexName, IndexDefinitionAttribute.IndexDefinitionMode.Index, group.Select(x => x.prop));
                        builder.Entity<TEntity>().HasIndex(keyExpression).HasName(@group.Key);
                    }
                }
            }
        }

        //private static void HasManyDefinition(ModelBuilder builder)
        //{
        //    var props = typeof(TEntity).GetProperties().Where(x => x.GetCustomAttribute<HasManyDefinitionAttribute>() != null);
        //    foreach (var prop in props)
        //    {
        //        var proptypeinf = prop.PropertyType;
        //        if (proptypeinf.IsAssignableTo(typeof(ICollection<>))) continue;
        //        var parentName = typeof(TEntity).Name;
        //        Type target = proptypeinf.GetType().GetGenericArguments()[0];
        //        var targetName = target.Name;
        //        var foreghnKeyColumnName = prop.GetCustomAttribute<HasManyDefinitionAttribute>().ForeighnKeyColumnName;

        //        var hasManyExpr = $"x=> x.{prop.Name}";
        //        //Expression<Func<TEntity, IEnumerable<TRelatedEntity>>>
        //        //builder.Entity<TEntity>().HasMany()
        //    }
        //}

        //private static void ManyToManyDefinition(ModelBuilder builder)
        //{
        //    var props = typeof(TEntity).GetProperties(BindingFlags.Public);  //TODO: cache this
        //    foreach (var prop in props)
        //    {
        //        var def = prop.GetCustomAttribute<ManyToManyDefinitionAttribute>();
        //        if (def != null)
        //        {
        //            var proptypeinf = prop.PropertyType;
        //            if (proptypeinf.IsAssignableTo(typeof(ICollection<>)))
        //            {
        //                Type target = proptypeinf.GetType().GetGenericArguments()[0];
        //                var names = new string[] { typeof(TEntity).Name, target.Name };
        //            }
        //            else
        //            {
        //                throw new ApplicationException("only for collection properties");
        //            }
        //        }
        //    }
        //}

        #endregion OnModelCreating
    }

    internal class InvalidPropertyName : ApplicationException
    {
        public InvalidPropertyName(Type type, string property) : base(CreateEcxeptionMessage(type, property))
        {
        }

        private static string CreateEcxeptionMessage(Type type, string property)
        {
            return $"Property {property} is not declared for type {type.FullName}";
        }
    }
}