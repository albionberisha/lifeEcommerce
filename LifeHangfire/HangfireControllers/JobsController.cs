using Hangfire;
using Hangfire.Common;
using lifeEcommerce.Models.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace LifeHangfire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : Controller
    {
        [HttpGet]
        [Route("login")]
        public String Login()
        {
            //Fire - and - Forget Job - this job is executed only once
            var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Welcome to Shopping World!"));

            return $"Job ID: {jobId}. Welcome mail sent to the user!";
        }

        [HttpGet]
        [Route("jobsfcheckout")]
        public String CheckoutJobs()
        {
            //Delayed Job - this job executed only once but not immedietly after some time.
            var jobId = BackgroundJob.Schedule(() => Console.WriteLine("You checkout new jobs into your checklist!"), TimeSpan.FromSeconds(20));

            return $"Job ID: {jobId}. You added one jobs into your checklist successfully!";
        }

        [HttpGet]
        [Route("jobspayment")]
        public String JobsPayment()
        {
            //Fire and Forget Job - this job is executed only once
            var parentjobId = BackgroundJob.Enqueue(() => Console.WriteLine("You have done your payment suceessfully!"));

            //Continuations Job - this job executed when its parent job is executed.
            BackgroundJob.ContinueJobWith(parentjobId, () => Console.WriteLine("Jobs receipt sent!"));

            return "You have done payment and receipt sent on your mail id!";
        }

        [HttpGet]
        [Route("dailyoffers")]
        public String DailyOffers()
        {
            //Recurring Job - this job is executed many times on the specified cron schedule
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Sent similar jobs offer and suuggestions"), Cron.Daily);
            return "offer sent!";
        }

        //Here we created different endpoints based on product scenarios like when a user login to the shopping site
        //it will get a welcome message on an immediate basis using fire and forget job.
        //After that, we created another endpoint related to the product checklist.That when the user adds a new
        //product to the checklist it will get notified and reminded after a few seconds like you are adding a new
        //product to your checklist.
        //In the third endpoint, we created a job for payment.When the user completes the process then it will get
        //an email immediately using Fire-and-Forget Job and later on when this job is executed the Continuation job
        //will get executed which is the child job and executed after the parent job will get executed.
        //Finally, In the last endpoint, we want to send special offers monthly basis for that we use a recurring job
        //that will execute continuously in the background after specified Cron conditions.
    }
}