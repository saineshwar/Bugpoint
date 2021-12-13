using System;
using BugPoint.Data.EFContext;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using BugPoint.Common;
using BugPoint.Model.MenuMaster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BugPoint.Data.Masters.Queries
{
    public class MastersQueries : IMastersQueries
    {
        private readonly BugPointContext _bugPointContext;
        private readonly IMemoryCache _cache;
        public MastersQueries(BugPointContext bugPointContext, IMemoryCache cache)
        {
            _bugPointContext = bugPointContext;
            _cache = cache;
        }

        public List<SelectListItem> ListofHardware()
        {
            var key = $"{AllMemoryCacheKeys.HardwareKey}";
            List<SelectListItem> listofdata;

            if (_cache.Get(key) == null)
            {
                listofdata = (from roles in _bugPointContext.HardwareModel.AsNoTracking()
                              orderby roles.Hardware ascending
                              select new SelectListItem()
                              {
                                  Text = roles.Hardware,
                                  Value = roles.Code.ToString()
                              }).ToList();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddYears(1),
                    Priority = CacheItemPriority.High
                };

                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                _cache.Set<List<SelectListItem>>(key, listofdata, cacheExpirationOptions);


            }
            else
            {
                listofdata = _cache.Get(key) as List<SelectListItem>; ;
            }


            if (listofdata == null)
            {
                listofdata = new List<SelectListItem>();
                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

            }
            return listofdata;
        }

        public List<SelectListItem> ListofPriority()
        {
            var key = $"{AllMemoryCacheKeys.PriorityKey}";
            List<SelectListItem> listofdata;

            if (_cache.Get(key) == null)
            {
                listofdata = (from roles in _bugPointContext.PriorityModel.AsNoTracking()

                              select new SelectListItem()
                              {
                                  Text = roles.PriorityName,
                                  Value = roles.Code.ToString()
                              }).ToList();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddYears(1),
                    Priority = CacheItemPriority.High
                };

                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });


                _cache.Set<List<SelectListItem>>(key, listofdata, cacheExpirationOptions);


            }
            else
            {
                listofdata = _cache.Get(key) as List<SelectListItem>; ;
            }

            if (listofdata == null)
            {
                listofdata = new List<SelectListItem>();
                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

            }
            return listofdata;
        }

        public List<SelectListItem> ListofSeverity()
        {
            var key = $"{AllMemoryCacheKeys.SeverityKey}";
            List<SelectListItem> listofdata;

            if (_cache.Get(key) == null)
            {
                listofdata = (from roles in _bugPointContext.SeverityModel.AsNoTracking()
                              orderby roles.Severity ascending
                              select new SelectListItem()
                              {
                                  Text = roles.Severity,
                                  Value = roles.Code.ToString()
                              }).ToList();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddYears(1),
                    Priority = CacheItemPriority.High
                };


                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                _cache.Set<List<SelectListItem>>(key, listofdata, cacheExpirationOptions);


            }
            else
            {
                listofdata = _cache.Get(key) as List<SelectListItem>; ;
            }



            if (listofdata == null)
            {
                listofdata = new List<SelectListItem>();
                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

            }

            return listofdata;
        }

        public List<SelectListItem> ListofResolution()
        {
            var key = $"{AllMemoryCacheKeys.ResolutionKey}";
            List<SelectListItem> listofdata;

            if (_cache.Get(key) == null)
            {
                listofdata = (from roles in _bugPointContext.ResolutionModel.AsNoTracking()

                              select new SelectListItem()
                              {
                                  Text = roles.Resolution,
                                  Value = roles.Code.ToString()
                              }).ToList();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddYears(1),
                    Priority = CacheItemPriority.High
                };

                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                _cache.Set<List<SelectListItem>>(key, listofdata, cacheExpirationOptions);


            }
            else
            {
                listofdata = _cache.Get(key) as List<SelectListItem>; ;
            }

            if (listofdata == null)
            {
                listofdata = new List<SelectListItem>();
                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

            }

            return listofdata;
        }

        public List<SelectListItem> ListofUserStatus()
        {
            var key = $"{AllMemoryCacheKeys.UserStatusKey}";
            List<SelectListItem> listofdata;

            if (_cache.Get(key) == null)
            {
                listofdata = (from roles in _bugPointContext.StatusModel.AsNoTracking()
                              where roles.ViewUser == true
                              select new SelectListItem()
                              {
                                  Text = roles.StatusName,
                                  Value = roles.Code.ToString()
                              }).ToList();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddYears(1),
                    Priority = CacheItemPriority.High
                };

                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                _cache.Set<List<SelectListItem>>(key, listofdata, cacheExpirationOptions);


            }
            else
            {
                listofdata = _cache.Get(key) as List<SelectListItem>; ;
            }

            if (listofdata == null)
            {
                listofdata = new List<SelectListItem>();
                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

            }

            return listofdata;
        }

        public List<SelectListItem> ListofReportStatus()
        {
            var key = $"{AllMemoryCacheKeys.ReporterStatusKey}";
            List<SelectListItem> listofdata;

            if (_cache.Get(key) == null)
            {
                listofdata = (from roles in _bugPointContext.StatusModel.AsNoTracking()
                              where roles.ViewReporter == true
                              select new SelectListItem()
                              {
                                  Text = roles.StatusName,
                                  Value = roles.Code.ToString()
                              }).ToList();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddYears(1),
                    Priority = CacheItemPriority.High
                };

                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                _cache.Set<List<SelectListItem>>(key, listofdata, cacheExpirationOptions);


            }
            else
            {
                listofdata = _cache.Get(key) as List<SelectListItem>; ;
            }

            if (listofdata == null)
            {
                listofdata = new List<SelectListItem>();
                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

            }

            return listofdata;
        }

        public List<SelectListItem> ListofStatus()
        {
            var key = $"{AllMemoryCacheKeys.StatusKey}";
            List<SelectListItem> listofdata;

            if (_cache.Get(key) == null)
            {
                listofdata = (from roles in _bugPointContext.StatusModel.AsNoTracking()
                              select new SelectListItem()
                              {
                                  Text = roles.StatusName,
                                  Value = roles.Code.ToString()
                              }).ToList();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddYears(1),
                    Priority = CacheItemPriority.High
                };

                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                _cache.Set<List<SelectListItem>>(key, listofdata, cacheExpirationOptions);


            }
            else
            {
                listofdata = _cache.Get(key) as List<SelectListItem>; ;
            }

            if (listofdata == null)
            {
                listofdata = new List<SelectListItem>();
                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

            }

            return listofdata;
        }

        public List<SelectListItem> ListofOperatingSystem()
        {
            var key = $"{AllMemoryCacheKeys.OperatingSystemKey}";
            List<SelectListItem> listofdata;

            if (_cache.Get(key) == null)
            {
                listofdata = (from roles in _bugPointContext.OperatingSystemModels.AsNoTracking()
                    orderby roles.OperatingSystemName ascending
                              select new SelectListItem()
                              {
                                  Text = roles.OperatingSystemName,
                                  Value = roles.Code.ToString()
                              }).ToList();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddYears(1),
                    Priority = CacheItemPriority.High
                };

                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                _cache.Set<List<SelectListItem>>(key, listofdata, cacheExpirationOptions);

            }
            else
            {
                listofdata = _cache.Get(key) as List<SelectListItem>; ;
            }


            if (listofdata == null)
            {
                listofdata = new List<SelectListItem>();
                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

            }

            return listofdata;
        }

        public List<SelectListItem> ListofVersions()
        {
            var key = $"{AllMemoryCacheKeys.VersionKey}";
            List<SelectListItem> listofdata;

            if (_cache.Get(key) == null)
            {
                listofdata = (from roles in _bugPointContext.VersionModel.AsNoTracking()

                              select new SelectListItem()
                              {
                                  Text = roles.VersionName,
                                  Value = roles.Code.ToString()
                              }).ToList();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddYears(1),
                    Priority = CacheItemPriority.High
                };



                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                _cache.Set<List<SelectListItem>>(key, listofdata, cacheExpirationOptions);


            }
            else
            {
                listofdata = _cache.Get(key) as List<SelectListItem>; ;
            }


            if (listofdata == null)
            {
                listofdata = new List<SelectListItem>();
                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

            }

            return listofdata;
        }

        public List<SelectListItem> ListofBrowser()
        {
            var key = $"{AllMemoryCacheKeys.BrowserKey}";
            List<SelectListItem> listofdata;

            if (_cache.Get(key) == null)
            {
                listofdata = (from roles in _bugPointContext.BrowserModel.AsNoTracking()
                              orderby roles.BrowserName ascending
                              select new SelectListItem()
                              {
                                  Text = roles.BrowserName,
                                  Value = roles.Code.ToString()
                              }).ToList();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddYears(1),
                    Priority = CacheItemPriority.High
                };


                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                _cache.Set<List<SelectListItem>>(key, listofdata, cacheExpirationOptions);


            }
            else
            {
                listofdata = _cache.Get(key) as List<SelectListItem>; ;
            }


            if (listofdata == null)
            {
                listofdata = new List<SelectListItem>();
                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

            }

            return listofdata;
        }

        public List<SelectListItem> ListofWebFramework()
        {
            var key = $"{AllMemoryCacheKeys.WebFrameworkKey}";
            List<SelectListItem> listofdata;

            if (_cache.Get(key) == null)
            {
                listofdata = (from roles in _bugPointContext.WebFrameworkModel.AsNoTracking()
                              orderby roles.WebFramework ascending
                              select new SelectListItem()
                              {
                                  Text = roles.WebFramework,
                                  Value = roles.Code.ToString()
                              }).ToList();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddYears(1),
                    Priority = CacheItemPriority.High
                };

                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });


                _cache.Set<List<SelectListItem>>(key, listofdata, cacheExpirationOptions);


            }
            else
            {
                listofdata = _cache.Get(key) as List<SelectListItem>; ;
            }


            if (listofdata == null)
            {
                listofdata = new List<SelectListItem>();
                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

            }
            return listofdata;
        }

        public List<SelectListItem> ListofEnvironments()
        {
            var key = $"{AllMemoryCacheKeys.EnvironmentKey}";
            List<SelectListItem> listofdata;

            if (_cache.Get(key) == null)
            {
                listofdata = (from roles in _bugPointContext.TestedEnvironmentModel.AsNoTracking()
                              orderby roles.TestedOn ascending
                              select new SelectListItem()
                              {
                                  Text = roles.TestedOn,
                                  Value = roles.Code.ToString()
                              }).ToList();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddYears(1),
                    Priority = CacheItemPriority.High
                };


                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                _cache.Set<List<SelectListItem>>(key, listofdata, cacheExpirationOptions);


            }
            else
            {
                listofdata = _cache.Get(key) as List<SelectListItem>; ;
            }


            if (listofdata == null)
            {
                listofdata = new List<SelectListItem>();
                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

            }

            return listofdata;
        }

        public List<SelectListItem> ListofBugTypes()
        {
            var key = $"{AllMemoryCacheKeys.BugTypesKey}";
            List<SelectListItem> listofdata;

            if (_cache.Get(key) == null)
            {
                listofdata = (from roles in _bugPointContext.BugTypesModel.AsNoTracking()
                              orderby roles.BugType ascending
                              select new SelectListItem()
                              {
                                  Text = roles.BugType,
                                  Value = roles.Code.ToString()
                              }).ToList();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddYears(1),
                    Priority = CacheItemPriority.High
                };


                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                _cache.Set<List<SelectListItem>>(key, listofdata, cacheExpirationOptions);


            }
            else
            {
                listofdata = _cache.Get(key) as List<SelectListItem>; ;
            }


            if (listofdata == null)
            {
                listofdata = new List<SelectListItem>();
                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

            }

            return listofdata;
        }

        public string GetPriorityNameBypriorityId(int? priorityId)
        {
            var priorityList = (from priority in _bugPointContext.PriorityModel.AsNoTracking()
                                where priority.PriorityId == priorityId
                                select priority.PriorityName).FirstOrDefault();
            return priorityList;
        }

        public string GetStatusBystatusId(int? statusId)
        {
            var priorityList = (from status in _bugPointContext.StatusModel.AsNoTracking()
                                where status.StatusId == statusId
                                select status.StatusName).FirstOrDefault();
            return priorityList;
        }
        public string GetResolutionByResolutionId(int? resolutionId)
        {
            var resolutionList = (from resolution in _bugPointContext.ResolutionModel.AsNoTracking()
                                  where resolution.ResolutionId == resolutionId
                                  select resolution.Resolution).FirstOrDefault();
            return resolutionList;
        }

        public List<SelectListItem> ListofDesignation()
        {
            var key = $"{AllMemoryCacheKeys.DesignationKey}";
            List<SelectListItem> listofdata;

            if (_cache.Get(key) == null)
            {
                listofdata = (from roles in _bugPointContext.DesignationModel.AsNoTracking()
                              orderby roles.Designation ascending
                              select new SelectListItem()
                              {
                                  Text = roles.Designation,
                                  Value = roles.DesignationId.ToString()
                              }).ToList();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddYears(1),
                    Priority = CacheItemPriority.High
                };

                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });


                _cache.Set<List<SelectListItem>>(key, listofdata, cacheExpirationOptions);


            }
            else
            {
                listofdata = _cache.Get(key) as List<SelectListItem>; ;
            }

            if (listofdata == null)
            {
                listofdata = new List<SelectListItem>();
                listofdata.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

            }
            return listofdata;
        }
    }
}