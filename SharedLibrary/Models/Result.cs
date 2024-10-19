using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Models
{
	public class Result : BaseEntity
	{
		public int Score { get; set; }
		public double ScorePercent { get; set; }

		public Guid VacancyId { get; set; }
		[ForeignKey(nameof(VacancyId))]
		public Vacancy Vacancy { get; set; }

		public Guid ApplierId { get; set; }
		[ForeignKey(nameof(ApplierId))]
		public ApplierForAdmin Applier { get; set; }

	}
}
