using BL.Data;
using BL.Enums;
using BL.Models;
using Microsoft.EntityFrameworkCore;
using System.Web.Mvc;

namespace BL
{
    public static class B
    {
        private static readonly ApplicationDbContext _context = new();


        #region Create
        
        public static void CreateCompetenceArea(CompetenceArea area)
        {
            if (area == null ||
                string.IsNullOrWhiteSpace(area.Name))
                return;

            CompetenceArea dbArea = new()
            {
                Name = area.Name
            };

            _context.CompetenceAreas.Add(dbArea);
            _context.SaveChanges();
        }

        public static void CreateCompetence(Competence competence)
        {
            if (competence == null ||
                competence.CompetenceArea.Id < 1 ||
                string.IsNullOrWhiteSpace(competence.Name))
                return;

            Competence dbCompetence = new()
            {
                Name = competence.Name,
                Description = competence.Description,
                Level = competence.Level,
                CompetenceArea = competence.CompetenceArea
            };

            _context.Competences.Add(dbCompetence);
            _context.SaveChanges();
        }

        public static void CreateResource(Resource resource)
        {
            if (resource == null ||
                resource.Competence.Id < 1 ||
                string.IsNullOrWhiteSpace(resource.DisplayText) ||
                string.IsNullOrWhiteSpace(resource.Link))
                return;

            Resource dbResource = new()
            {
                DisplayText = resource.DisplayText,
                Link = resource.Link,
                Competence = resource.Competence
            };

            _context.Resources.Add(dbResource);
            _context.SaveChanges();
        }

        #endregion

        #region Read

        public static ApplicationUser? GetUser(string userId)
        {
            return _context.Users.Find(userId);
        }
        public static Competence? GetCompetence(int competenceId)
        {
            return _context.Competences.Find(competenceId);
        }
        public static Competence? GetCompetenceFromResourceId(int resourceId)
        {
            return _context.Resources.Include(x => x.Competence).FirstOrDefault(x => x.Id == resourceId)?.Competence;
        }
        public static CompetenceArea? GetCompetenceArea(int areaId)
        {
            return _context.CompetenceAreas.Find(areaId);
        }
        public static CompetenceArea? GetCompetenceAreaFromCompetenceId(int competenceId)
        {
            return _context.Competences.Include(x => x.CompetenceArea).FirstOrDefault(x => x.Id == competenceId)?.CompetenceArea;
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
        public static Resource? GetResource(int resourceId)
        {
            return _context.Resources.Find(resourceId);
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
        public static List<Competence> GetAllCompetencesFromArea(int areaId)
        {
            return _context.Competences.Where(x => x.CompetenceArea.Id == areaId).ToList();
        }
        public static List<UserCompetence> GetUsersUserCompetences(string userId)
        {
            return _context.UserCompetences.Where(x => x.User.Id == userId).ToList();
        }
        public static List<UserCompetence> GetAllUserCompetences()
        {
            return _context.UserCompetences.ToList();
        }
        public static List<Resource> GetResourcesFromCompetence(int competenceId)
        {
            return _context.Resources.Where(x => x.Competence.Id == competenceId).ToList();
        }
        public static List<ApplicationUser> GetAllApplicationUsers()
        {
            return _context.Users.ToList();
        }

        #endregion

        #region Update

        public static void UpdateCompetenceArea(CompetenceArea area)
        {
            if (area == null ||
                area.Id < 1 ||
                string.IsNullOrWhiteSpace(area.Name))
                return;

            var dbArea = GetCompetenceArea(area.Id);
            if (dbArea == null)
                return;

            dbArea.Name = area.Name;
            _context.SaveChanges();
        }

        public static void UpdateCompetence(Competence competence)
        {
            if (competence == null ||
                competence.Id < 1 ||
                string.IsNullOrWhiteSpace(competence.Name))
                return;

            var dbCompetence = GetCompetence(competence.Id);
            if (dbCompetence == null)
                return;

            dbCompetence.Name = competence.Name;
            dbCompetence.Description = competence.Description;
            dbCompetence.Level = competence.Level;
            _context.SaveChanges();
        }

        public static void UpdateResource(Resource resource)
        {
            if (resource == null ||
                resource.Id < 1 ||
                string.IsNullOrWhiteSpace(resource.DisplayText) ||
                string.IsNullOrWhiteSpace(resource.Link))
                return;

            var dbResource = GetResource(resource.Id);
            if (dbResource == null)
                return;

            dbResource.DisplayText = resource.DisplayText;
            dbResource.Link = resource.Link;
            _context.SaveChanges();
        }

        public static void InvertUserCompetencePinState(string userid, int competenceId)
        {
            UserCompetence? userCompetence = GetUserCompetence(userid, competenceId);
            if (userCompetence == null)
                return;

            userCompetence.Pinned = !userCompetence.Pinned;
            _context.SaveChanges();
        }
        public static void UpdateCompetenceState(UserCompetence userCompetence, CompetenceState competenceState)
        {
            if (userCompetence == null)
                return;
            UserCompetence userCompetenceFromDb = _context.UserCompetences.FirstOrDefault(x => x.Id == userCompetence.Id);
            userCompetenceFromDb.State = competenceState;
            _context.SaveChanges();
        }

        #endregion

        #region Delete

        public static void DeleteCompetenceArea(int areaId)
        {
            var competenceAreaToDelete = GetCompetenceArea(areaId);
            if (competenceAreaToDelete == null)
                return;

            var competencesToDelete = _context.Competences.Where(x => x.CompetenceArea.Id == areaId);
            foreach (var competence in competencesToDelete)
            {
                DeleteCompetence(competence.Id);
            }
            _context.CompetenceAreas.Remove(competenceAreaToDelete);
            _context.SaveChanges();
        }

        public static void DeleteCompetence(int competenceId)
        {
            var competenceToDelete = GetCompetence(competenceId);
            if (competenceToDelete == null)
                return;

            var userCompetencesToDelete = _context.UserCompetences.Where(x => x.Competence.Id == competenceId);
            var resourcesToDelete = _context.Resources.Where(x => x.Competence.Id == competenceId);
            _context.UserCompetences.RemoveRange(userCompetencesToDelete);
            _context.Resources.RemoveRange(resourcesToDelete);
            _context.Competences.Remove(competenceToDelete);
            _context.SaveChanges();
        }
        
        public static void DeleteResource(int resourceId)
        {
            var resourceToDelete = GetResource(resourceId);
            if (resourceToDelete == null)
                return;

            _context.Resources.Remove(resourceToDelete);
            _context.SaveChanges();
        }

        // TODO: Delete UserCompetences when User gets deleted

        #endregion


    }
}
