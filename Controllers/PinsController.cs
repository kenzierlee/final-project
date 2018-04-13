using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using keepr.Models;
using keepr.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace keepr.Controllers
{
    [Route("api/[controller]")]
    public class PinsController : Controller
    {
        private readonly PinRepository _pinRepo;
        public PinsController(PinRepository pinRepo)
        {
            _pinRepo = pinRepo;
        }
        //get all pins associated to the given boardId
        [HttpGet]
        public IEnumerable<Pin> GetPins()
        {
            try
            {
                return _pinRepo.GetPins();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return null;
            }
        }
        [HttpGet("{boardId}")]
        public IEnumerable<Pin> GetBoardPins(string boardId)
        {
            try
            {
                return _pinRepo.GetBoardPins(boardId);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return null;
            }
        }
        [HttpGet("userpins")]
        public IEnumerable<Pin> GetUserPins()
        {
            try
            {
                string userId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
                return _pinRepo.GetUserPins(userId);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return null;
            }
        }
        [HttpPost]
        public Pin AddPin([FromBody]Pin pinData)
        {
            try
            {
                var userId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
                pinData.UserId = userId;
                pinData.Views = 0;
                pinData.Saves = 0;
                return _pinRepo.AddPin(pinData);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return null;
            }
        }
        [HttpPost("savepin")]
        public BoardPin SavePin([FromBody]BoardPin boardPinData)
        {
            try
            {
                var userId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name)
                .Select(c => c.Value).SingleOrDefault();
                boardPinData.UserId = userId;
                return _pinRepo.SavePin(boardPinData);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return null;
            }
        }
        [HttpPut("{pinId}")]
        public Pin EditPin(string pinId, [FromBody] Pin pinData)
        {
            try
            {
                var userId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
                Pin pin = _pinRepo.GetPinById(userId, pinId);
                return _pinRepo.EditPin(pin, pinData);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return null;
            }
        }
        [HttpPut("views/{pinId}")]
        public string UpdateViews(string pinId)
        {
            try
            {
                return _pinRepo.UpdateViews(pinId);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return null;
            }
        }
        [HttpPut("saves/{pinId}")]
        public string UpdataSaves(string pinId)
        {
            try
            {
                return _pinRepo.UpdateSaves(pinId);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return null;
            }
        }
        [HttpDelete("{pinId}")]
        public string DeletePin(string pinId)
        {
            try
            {
                return _pinRepo.DeletePin(pinId);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return null;
            }
        }
        [HttpDelete("boardpin/{boardPinId}")]
        public string DeleteBoardPin(string boardPinId)
        {
            try
            {
                return _pinRepo.DeleteBoardPin(boardPinId);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}