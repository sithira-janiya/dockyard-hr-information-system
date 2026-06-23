using System;
using System.Collections.Generic;
using System.Web.Http;
using WebApplication1.BusinessLayer;
using WebApplication1.DataAccess;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Models.RequestApiModels;

namespace WebApplication1.Controllers
{
    /// API controller for employee distance‑related operations.
    [RoutePrefix("api/EmployeeDistance")]
    public class EmployeeDistanceController : ApiController
    {
        private readonly IEmployee _employeeService;

        public EmployeeDistanceController()
        {
            // Dependency resolution
            _employeeService = new DAEmployee();
        }


        /// Returns the distance from the employee's location to the workplace.

        [HttpGet]
        [Route("GetDistance/{employeeId}")]
        public IHttpActionResult GetEmployeeDistance(string employeeId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(employeeId))
                    return BadRequest("Employee ID is required.");

                var request = new EmployeeRequestAPI
                {
                    EmployeeID = employeeId,
                    IncludeDistance = true
                };

                var response = _employeeService.GetEmployeeDistanceToWorkplace(request);

                if (response.StatusCode == 200)
                {
                    return Ok(new
                    {
                        Status = "Success",
                        StatusCode = response.StatusCode,
                        Message = response.Result,
                        Data = response.ResultSet
                    });
                }
                else if (response.StatusCode == 404)
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(response.Result);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// Returns distance along with lat/long of both employee and workplace.
        [HttpGet]
        [Route("GetDistanceWithDetails/{employeeId}")]
        public IHttpActionResult GetEmployeeDistanceWithDetails(string employeeId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(employeeId))
                    return BadRequest("Employee ID is required.");

                var request = new EmployeeRequestAPI
                {
                    EmployeeID = employeeId,
                    IncludeDistance = true
                };

                var response = _employeeService.GetEmployeeDistanceToWorkplace(request);

                if (response.StatusCode == 200)
                {
                    var distanceData = response.ResultSet as List<DAEmployee.EmployeeDistanceModel>;
                    if (distanceData != null && distanceData.Count > 0)
                    {
                        var data = distanceData[0];
                        return Ok(new
                        {
                            Status = "Success",
                            StatusCode = 200,
                            Message = response.Result,
                            Data = new
                            {
                                Employee = data.Employee,
                                Distance = new
                                {
                                    Kilometers = Math.Round(data.DistanceToWorkplace, 2),
                                    Formatted = data.FormattedDistance,
                                    WorkplaceLocation = data.WorkplaceLocation
                                },
                                Coordinates = new
                                {
                                    Employee = new { data.EmployeeLatitude, data.EmployeeLongitude },
                                    Workplace = new { data.WorkplaceLatitude, data.WorkplaceLongitude }
                                }
                            }
                        });
                    }
                }
                else if (response.StatusCode == 404)
                {
                    return NotFound();
                }
                return BadRequest(response.Result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        /// Calculates distance for every employee and returns the list.
        [HttpGet]
        [Route("GetAllDistances")]
        public IHttpActionResult GetAllEmployeeDistances()
        {
            try
            {
                var request = new EmployeeRequestAPI();
                var response = _employeeService.GetEmployeeDetails(request);

                if (response.StatusCode == 200)
                {
                    var employeeList = response.ResultSet as List<EmployeeModel>;
                    if (employeeList != null && employeeList.Count > 0)
                    {
                        var distanceList = new List<object>();

                        foreach (var emp in employeeList)
                        {
                            var distance = DistanceCalculator.GetDistanceToWorkplace(emp.LocationID);
                            var coords = DistanceCalculator.GetLocationCoordinates(emp.LocationID);

                            distanceList.Add(new
                            {
                                Employee = new
                                {
                                    emp.EmployeeID,
                                    emp.EmployeeName,
                                    emp.EmployeeAddress,
                                    emp.LocationName,
                                    emp.DepartmentName,
                                    emp.DesignationName
                                },
                                Distance = new
                                {
                                    Kilometers = Math.Round(distance, 2),
                                    Formatted = DistanceCalculator.FormatDistance(distance)
                                },
                                Coordinates = new { Latitude = coords.Lat, Longitude = coords.Lon },
                                WorkplaceLocation = DistanceCalculator.GetWorkplaceName()
                            });
                        }

                        return Ok(new
                        {
                            Status = "Success",
                            StatusCode = 200,
                            Message = $"Retrieved distances for {distanceList.Count} employees.",
                            TotalEmployees = distanceList.Count,
                            Data = distanceList
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            Status = "Success",
                            StatusCode = 200,
                            Message = "No employees found.",
                            TotalEmployees = 0,
                            Data = new List<object>()
                        });
                    }
                }
                return BadRequest(response.Result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <param name="radius">Radius in kilometers (e.g., 50)</param>
        [HttpGet]
        [Route("GetWithinRadius")]
        public IHttpActionResult GetEmployeesWithinRadius([FromUri] decimal radius)
        {
            try
            {
                if (radius <= 0)
                    return BadRequest("Radius must be greater than 0.");

                var request = new EmployeeRequestAPI();
                var response = _employeeService.GetEmployeeDetails(request);

                if (response.StatusCode == 200)
                {
                    var employeeList = response.ResultSet as List<EmployeeModel>;
                    var withinRadius = new List<object>();

                    if (employeeList != null)
                    {
                        foreach (var emp in employeeList)
                        {
                            var distance = DistanceCalculator.GetDistanceToWorkplace(emp.LocationID);
                            if (distance <= radius)
                            {
                                var coords = DistanceCalculator.GetLocationCoordinates(emp.LocationID);
                                withinRadius.Add(new
                                {
                                    Employee = new
                                    {
                                        emp.EmployeeID,
                                        emp.EmployeeName,
                                        emp.EmployeeAddress,
                                        emp.LocationName,
                                        emp.DepartmentName,
                                        emp.DesignationName
                                    },
                                    Distance = new
                                    {
                                        Kilometers = Math.Round(distance, 2),
                                        Formatted = DistanceCalculator.FormatDistance(distance)
                                    },
                                    Coordinates = new { Latitude = coords.Lat, Longitude = coords.Lon }
                                });
                            }
                        }
                    }

                    return Ok(new
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = $"Found {withinRadius.Count} employees within {radius} km.",
                        RadiusInKm = radius,
                        TotalEmployees = withinRadius.Count,
                        Data = withinRadius
                    });
                }
                return BadRequest(response.Result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}