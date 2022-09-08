using DikkeTennisLijst.Core.Equatables;

namespace DikkeTennisLijst.Core.Shared.Specifications
{
    public class SpecificationEmailTemplate : SpecificationEquatable<EmailTemplate>
    {
        public SpecificationEmailTemplate(string fileName) : base(x => x.TemplateFileName == fileName)
        {
        }
    }
}