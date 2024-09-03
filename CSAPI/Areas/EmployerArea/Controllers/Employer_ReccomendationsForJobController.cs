using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CSAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using CSAPI.Areas.EmployerArea.Models;

namespace CSAPI.Areas.EmployerArea.Controllers
{
    [Area("EmployerArea")]
    [Route("api/[controller]/")]
    [ApiController]
    [Authorize(Roles = "Employer")]
    public class Employer_ReccomendationsForJobController : ControllerBase
    {
        private readonly IJobsRepo _jobsRepo;
        private readonly IEmployerAreaRepo _eRepo;

        public Employer_ReccomendationsForJobController(IJobsRepo jobsRepo, IEmployerAreaRepo empRepo)
        {
            _jobsRepo = jobsRepo;
            _eRepo = empRepo;
        }

        [HttpGet("RecommendationForEachJobByJobID/{id}")]
        public async Task<List<JobSeeker>> GetRecommendations(int id)
        {
            List<JobSeeker> recommendedList = new List<JobSeeker>();
            Tuple<JobSeeker, int> recommendedrow;
            List<Tuple<JobSeeker, int>> recommendedListCondition = new List<Tuple<JobSeeker, int>>();
            List<JobSeeker> IndustryRec = new List<JobSeeker>();
            List<JobSeeker> SpecialRec = new List<JobSeeker>();
            List<JobSeeker> SkillRec = new List<JobSeeker>();
            List<Tuple<JobSeeker, int>> PrefSkillRec = new List<Tuple<JobSeeker, int>>();
            int pref = 0;

            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            int Empid = _eRepo.GetEmpID(userIdCookie);

            var job = await _jobsRepo.FindByIdAsync(id);
            if (job != null && job.EmployerID == Empid)
            {
                IndustryRec = await _eRepo.GetRecommendationByIndustry(id);
                SpecialRec = await _eRepo.GetRecommendationBySpecialization(id);
                PrefSkillRec = await _eRepo.GetRecommendationBySkills(id);
                SkillRec = PrefSkillRec.Select(x => x.Item1).ToList();

                foreach (var i in SkillRec)
                {
                    pref = 0;
                    if (IndustryRec.Contains(i) && SpecialRec.Contains(i))
                        pref = 1;
                    else if (IndustryRec.Contains(i) || SpecialRec.Contains(i))
                        pref = 2;
                    else pref = 3;
                    recommendedrow = new Tuple<JobSeeker, int>(i, pref);
                    recommendedListCondition.Add(recommendedrow);
                }
                foreach (var i in SpecialRec)
                {
                    if (IndustryRec.Contains(i))
                        pref = 2;
                    else pref = 3;
                    if (!SkillRec.Contains(i))
                    {
                        recommendedrow = new Tuple<JobSeeker, int>(i, pref);
                        recommendedListCondition.Add(recommendedrow);
                    }
                }
                foreach (var i in IndustryRec)
                {
                    if (!SkillRec.Contains(i) && !SpecialRec.Contains(i))
                    {
                        pref = 3;
                        recommendedrow = new Tuple<JobSeeker, int>(i, pref);
                        recommendedListCondition.Add(recommendedrow);
                    }
                }

                recommendedList = recommendedListCondition.Select(x => x.Item1).ToList();
                return await Task.FromResult(recommendedList);
            }
            else
                return null;
        }

    }
}
