﻿using Daily_Metting.Models;

namespace Daily_Metting.Repositories.Attainements
{
    public interface IAttainementRepository
    {
        List<Attainement> AllAttainement { get; }
        void AddAttainement(Attainement attainement);
        void DeleteAttainementsBySubmission(Submission submission);
        List<string> GetProjectList(APU aPU, DateTime date);
        List<Attainement> GetAttainementsBySubmission(Submission submission);
        Attainement GetAttainementsAverage(string Project_name, DateTime date);
        List<Attainement> GetAttainementBySubmission(int submissionId);
        bool isAttainementsExist(DateTime date);
        List<Attainement> GetAttainementsBySubmission_Apu(Submission submission, APU aPU);

    }
}
