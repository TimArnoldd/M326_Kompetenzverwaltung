using BL.Data;
using BL.Models;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public static class B
    {
        private static readonly ApplicationDbContext _context = new();


        #region Create
        
        public static ApplicationUser? GetUser(string id)
        {
            return _context.Users.Find(id);
        }
        public static Competence? GetCompetence(int id)
        {
            return _context.Competences.Find(id);
        }
        public static CompetenceArea? GetCompetenceArea(int id)
        {
            return _context.CompetenceAreas.Find(id);
        }
        public static UserCompetence? GetUserCompetence(string userId, int competenceId)
        {
            UserCompetence? userCompetence = _context.UserCompetences.FirstOrDefault(x => x.User.Id == userId && x.Competence.Id == competenceId);

            if (userCompetence == null)
            {
                ApplicationUser? user = GetUser(userId);
                Competence? competence = GetCompetence(competenceId);
                if (user == null ||
                    competence == null)
                    return null;

                userCompetence = new()
                {
                    User = user,
                    Competence = competence
                };
                _context.UserCompetences.Add(userCompetence);
                _context.SaveChanges();
            }
            return userCompetence;
        }


        public static List<CompetenceArea> GetAllAreas()
        {
            return _context.CompetenceAreas.ToList();
        }
        public static List<CompetenceArea> GetAllAreasWithCompetencesLoaded()
        {
            return _context.CompetenceAreas.Include(x => x.Competences).ToList();
        }
        public static List<Competence> GetAllCompetences()
        {
            return _context.Competences.ToList();
        }
        public static List<UserCompetence> GetUsersUserCompetences(string userId)
        {
            return _context.UserCompetences.Where(x => x.User.Id == userId).ToList();
        }
        public static List<UserCompetence> GetAllUserCompetences()
        {
            return _context.UserCompetences.ToList();
        }
        public static List<ApplicationUser> GetAllApplicationUsers()
        {
            return _context.Users.ToList();
        }

        #endregion

        #region Read

        #endregion

        #region Update

        public static void InvertUserCompetencePinState(string userid, int competenceId)
        {
            UserCompetence? userCompetence = GetUserCompetence(userid, competenceId);
            if (userCompetence == null)
                return;

            userCompetence.Pinned = !userCompetence.Pinned;
            _context.SaveChanges();
        }

        #endregion

        #region Delete

        #endregion


    }
}
