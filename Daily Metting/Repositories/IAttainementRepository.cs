using Daily_Metting.Models;

namespace Daily_Metting.Repositories
{
    public interface IAttainementRepository
    {
        List<Attainement> AllAttainement { get; }
        void AddAttainement(Attainement attainement);
        void DeleteAttainementsBySubmission(Submission submission);
        List<string> GetProjectList(APU aPU);
        List<Attainement> GetAttainementsBySubmission(Submission submission);
        Attainement GetAttainementsAverage(string Project_name,DateTime date);
        List<Attainement> GetAttainementBySubmission(int submissionId);
        bool isAttainementsExist(DateTime date);
    }
}
