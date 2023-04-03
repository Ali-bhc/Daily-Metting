using Daily_Metting.Models;

namespace Daily_Metting.Repositories
{
    public interface IAPURepository
    {
        List<APU> AllApu { get; }
        APU GetAPUByName (string name);
        void AddAPU(APU aPU);
        void UpdateAPU(int aPUId , double Attainement_min , double Attainement_Max);
    }
}
