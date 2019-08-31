namespace Borg.Infrastructure.Core
{
    public class ResistanceIsFutile : IResistanceIsFutile
    {
        public virtual string Identifier => GetType().Name.SplitUpperCaseToWords();
    }
}