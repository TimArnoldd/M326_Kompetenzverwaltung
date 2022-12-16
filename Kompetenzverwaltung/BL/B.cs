using BL.Data;
using BL.Enums;
using BL.Models;
using Microsoft.EntityFrameworkCore;
using System.Web.Mvc;

namespace BL
{
    public class B
    {
        private ApplicationDbContext _context;

        public B()
        {
            _context = new();
        }
        public B(ApplicationDbContext context)
        {
            _context = context;
        }


        #region Create
        
        public void CreateCompetenceArea(CompetenceArea area)
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

        public void CreateCompetence(Competence competence)
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

        public void CreateResource(Resource resource)
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

        public ApplicationUser? GetUser(string userId)
        {
            return _context.Users.Find(userId);
        }
        public Competence? GetCompetence(int competenceId)
        {
            return _context.Competences.Find(competenceId);
        }
        public Competence? GetCompetenceFromResourceId(int resourceId)
        {
            return _context.Resources.Include(x => x.Competence).FirstOrDefault(x => x.Id == resourceId)?.Competence;
        }
        public CompetenceArea? GetCompetenceArea(int areaId)
        {
            return _context.CompetenceAreas.Find(areaId);
        }
        public CompetenceArea? GetCompetenceAreaFromCompetenceId(int competenceId)
        {
            return _context.Competences.Include(x => x.CompetenceArea).FirstOrDefault(x => x.Id == competenceId)?.CompetenceArea;
        }
        public UserCompetence? GetUserCompetence(string userId, int competenceId)
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
        public Resource? GetResource(int resourceId)
        {
            return _context.Resources.Find(resourceId);
        }


        public IEnumerable<CompetenceArea> GetAllAreas()
        {
            return _context.CompetenceAreas;
        }
        public IEnumerable<CompetenceArea> GetAllAreasWithCompetencesLoaded()
        {
            return _context.CompetenceAreas.Include(x => x.Competences);
        }
        public IEnumerable<Competence> GetAllCompetences()
        {
            return _context.Competences;
        }
        public IEnumerable<Competence> GetAllCompetencesFromArea(int areaId)
        {
            return _context.Competences.Where(x => x.CompetenceArea.Id == areaId);
        }
        public IEnumerable<UserCompetence> GetUsersUserCompetences(string userId)
        {
            return _context.UserCompetences.Where(x => x.User.Id == userId);
        }
        public IEnumerable<UserCompetence> GetAllUserCompetences()
        {
            return _context.UserCompetences;
        }
        public IEnumerable<Resource> GetAllResources()
        {
            return _context.Resources;
        }
        public IEnumerable<Resource> GetResourcesFromCompetence(int competenceId)
        {
            return _context.Resources.Where(x => x.Competence.Id == competenceId);
        }
        public IEnumerable<ApplicationUser> GetAllApplicationUsers()
        {
            return _context.Users;
        }

        #endregion

        #region Update

        public void UpdateCompetenceArea(CompetenceArea area)
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

        public void UpdateCompetence(Competence competence)
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

        public void UpdateResource(Resource resource)
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

        public void InvertUserCompetencePinState(string userid, int competenceId)
        {
            UserCompetence? userCompetence = GetUserCompetence(userid, competenceId);
            if (userCompetence == null)
                return;

            userCompetence.Pinned = !userCompetence.Pinned;
            _context.SaveChanges();
        }
        public void UpdateCompetenceState(UserCompetence userCompetence)
        {
            var dbUserCompetence = GetUserCompetence(userCompetence.User.Id, userCompetence.Competence.Id);
            if (dbUserCompetence == null)
                return;
            dbUserCompetence.State = userCompetence.State;
            _context.SaveChanges();
        }

        #endregion

        #region Delete

        public void DeleteCompetenceArea(int areaId)
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

        public void DeleteCompetence(int competenceId)
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
        
        public void DeleteResource(int resourceId)
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
