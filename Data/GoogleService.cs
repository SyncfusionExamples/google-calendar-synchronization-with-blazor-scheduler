using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace BlazorScheduler.Data
{
    public class GoogleService
    {
        string[] Scopes = { CalendarService.Scope.Calendar };
        string ApplicationName = "Google Calendar Sync with Blazor Scheduler";
        CalendarService Service;
        public GoogleService()
        {
            UserCredential credential;
            using (FileStream stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, Scopes, "admin", CancellationToken.None, new FileDataStore("token.json", true)).Result;
            }
            Service = new CalendarService(new BaseClientService.Initializer() { HttpClientInitializer = credential, ApplicationName = ApplicationName });
        }

        public List<Appointment> GetEvents(DateTime date)
        {
            try
            {
                EventsResource.ListRequest request = Service.Events.List("primary");
                request.TimeMin = date;
                request.MaxResults = 2500;
                Events events = request.Execute();
                List<Appointment> eventDatas = new List<Appointment>();
                if (events.Items != null && events.Items.Count > 0)
                {
                    foreach (Event eventItem in events.Items)
                    {
                        if (eventItem.Start == null && eventItem.Status == "cancelled")
                        {
                            continue;
                        }
                        DateTime start;
                        DateTime end;
                        if (string.IsNullOrEmpty(eventItem.Start.Date))
                        {
                            start = (DateTime)eventItem.Start.DateTime;
                            end = (DateTime)eventItem.End.DateTime;
                        }
                        else
                        {
                            start = Convert.ToDateTime(eventItem.Start.Date);
                            end = Convert.ToDateTime(eventItem.End.Date);
                        }
                        Appointment eventData = new Appointment()
                        {
                            Id = eventItem.Id,
                            Subject = eventItem.Summary,
                            StartTime = start,
                            EndTime = end,
                            Location = eventItem.Location,
                            Description = eventItem.Description,
                            IsAllDay = !string.IsNullOrEmpty(eventItem.Start.Date)
                        };
                        eventDatas.Add(eventData);
                    }
                }
                return eventDatas;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Appointment>();
            }
        }

        public void InsertEvent(Appointment eventData)
        {
            try
            {
                EventDateTime start;
                EventDateTime end;
                if (eventData.IsAllDay)
                {
                    start = new EventDateTime() { Date = eventData.StartTime.ToString("yyyy-MM-dd") };
                    end = new EventDateTime() { Date = eventData.EndTime.ToString("yyyy-MM-dd") };
                }
                else
                {
                    start = new EventDateTime() { DateTime = eventData.StartTime, TimeZone = "UTC" };
                    end = new EventDateTime() { DateTime = eventData.EndTime, TimeZone = "UTC" };
                }
                Event eventItem = new Event()
                {
                    Summary = eventData.Subject,
                    Start = start,
                    End = end,
                    Location = eventData.Location,
                    Description = eventData.Description,
                    Created = DateTime.Now.ToUniversalTime()
                };
                EventsResource.InsertRequest addEvent = Service.Events.Insert(eventItem, "primary");
                addEvent.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void UpdateEvent(Appointment eventData)
        {
            try
            {
                var editedEvent = Service.Events.Get("primary", eventData.Id).Execute();
                editedEvent.Summary = eventData.Subject;
                editedEvent.Location = eventData.Location;
                editedEvent.Description = eventData.Description;
                editedEvent.Updated = DateTime.Now.ToUniversalTime();
                if (eventData.IsAllDay)
                {
                    editedEvent.Start = new EventDateTime() { Date = eventData.StartTime.ToString("yyyy-MM-dd") };
                    editedEvent.End = new EventDateTime() { Date = eventData.EndTime.ToString("yyyy-MM-dd") };
                }
                else
                {
                    editedEvent.Start = new EventDateTime() { DateTime = eventData.StartTime };
                    editedEvent.End = new EventDateTime() { DateTime = eventData.EndTime };
                }
                var updatedEvent = Service.Events.Update(editedEvent, "primary", editedEvent.Id);
                updatedEvent.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void RemoveEvent(string id)
        {
            try
            {
                EventsResource.DeleteRequest deleteEvent = Service.Events.Delete("primary", id);
                deleteEvent.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
