using System;
using System.Web.Mvc;
using YourProjectName.Models; // Replace with your actual project's namespace

namespace YourProjectName.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities(); // Replace with your actual database context name

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,FullCoverage,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                // a. Start with a base of $50 / month.
                decimal monthlyTotal = 50;

                // Calculate age based on DateOfBirth
                int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
                if (insuree.DateOfBirth.Date > DateTime.Now.AddYears(-age)) age--;

                // b, c, d. Age checks
                if (age <= 18)
                {
                    monthlyTotal += 100;
                }
                else if (age >= 19 && age <= 25)
                {
                    monthlyTotal += 50;
                }
                else if (age >= 26)
                {
                    monthlyTotal += 25;
                }

                // e & f. Car year checks
                if (insuree.CarYear < 2000)
                {
                    monthlyTotal += 25;
                }
                else if (insuree.CarYear > 2015)
                {
                    monthlyTotal += 25;
                }

                // g & h. Car Make and Model checks
                if (insuree.CarMake.ToLower() == "porsche")
                {
                    monthlyTotal += 25;

                    if (insuree.CarModel.ToLower() == "carrera")
                    {
                        monthlyTotal += 25;
                    }
                }

                // i. Add $10 for every speeding ticket
                monthlyTotal += (insuree.SpeedingTickets * 10);

                // j. If user has ever had a DUI, add 25%
                if (insuree.DUI)
                {
                    monthlyTotal = monthlyTotal * 1.25m;
                }

                // k. If it's full coverage, add 50%
                if (insuree.FullCoverage)
                {
                    monthlyTotal = monthlyTotal * 1.50m;
                }

                // Assign the calculated amount to the model property
                insuree.Quote = monthlyTotal;

                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }
    }
}
