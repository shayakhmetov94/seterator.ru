﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.EntityFrameworkCore;
using Rauthor.Models;
using Rauthor.ViewModels;

namespace Rauthor.Controllers
{
    public class JuryController : Controller
    {
        DatabaseContext database;
        public JuryController(DatabaseContext database)
        {
            this.database = database;
        }
        
        /// <param name="guid">Guid участника</param>
        [HttpGet]
        [Authorize]
        public IActionResult Assessment([FromRoute] Guid guid)
        {
            var participant = database.Participants
                .Include(x => x.User)
                .Include(x => x.Poems)
                .Include(x => x.Competition)
                .First(x => x.Guid == guid);
            var assesment = database.ParticipantAssessments.Where(x => x.ParticipantGuid == participant.Guid).FirstOrDefault();
            var model = new AssessmentModel()
            {
                CompetitionTitle = participant.Competition.Titile,
                Participant = participant,
                AuthorName = participant.User.Login,
                Assessment = assesment
            };
            return View(model);

        }

        [HttpGet]
        [Authorize]
        public IActionResult Assessment()
        {
            var participant = database.Participants
                .Include(x => x.User)
                .Include(x => x.Poems)
                .Include(x => x.Competition)
                .Where(x => (database.ParticipantAssessments.FirstOrDefault(a => a.ParticipantGuid == x.Guid) == null))
                .First();
            return Redirect($"/Jury/Assessment/{participant.Guid}");

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid">Guid участника</param>
        /// <param name="assessment">Оценка</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult Assessment([FromRoute] [FromQuery] Guid guid, [FromForm] ParticipantAssessment assessment)
        {
            var participant = database.Participants.First(x => x.Guid == guid);
            database.ParticipantAssessments.Add(new ParticipantAssessment()
            {
                Assessment = assessment.Assessment,
                ParticipantGuid = guid
            });
            database.SaveChanges();
            return RedirectToAction("Assessment", new { guid = guid });
        }
    }
}
