using System.Collections.Generic;

namespace Agenda.Shared
{
    public interface IValidate
    {
        IList<ErrorViewModel> Validations { get; set; }

        bool IsValid { get; }

        void Validate();
    }
}
