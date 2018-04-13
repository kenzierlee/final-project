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
    public class BoardsController : Controller
    {
        private readonly BoardRepository _boardRepo;
        public BoardsController(BoardRepository boardRepo)
        {
            _boardRepo = boardRepo;
        }
        [HttpGet]
        public IEnumerable<Board> GetBoards()
        {
            var userId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name)
                .Select(c => c.Value).SingleOrDefault();
            return _boardRepo.getBoardsByUserId(userId);
        }
        [HttpGet("{id}")]
        public Board GetBoard(string id)
        {
            try
            {
                var userId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
                return _boardRepo.GetBoardById(userId, id);
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.Message);
                return null;
            }
        }
        [HttpPost]
        public Board AddBoard([FromBody]Board boardData)
        {
            var userId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name)
                .Select(c => c.Value).SingleOrDefault();
            boardData.UserId = userId;
            try
            {
                return _boardRepo.AddBoard(boardData);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return null;
            }
        }
        [Authorize]
        [HttpPut("{id}")]
        public Board UpdateBoard(string id, [FromBody] Board boardData)
        {
            var userId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name)
                .Select(c => c.Value).SingleOrDefault();
            Board board = _boardRepo.GetBoardById(userId, id);
            if (board.UserId == userId)
            {
                boardData.Id = board.Id;
                return _boardRepo.UpdateBoard(board, boardData);
            }
            else
            {
                return null;
            }
        }
        [Authorize]
        [HttpDelete("{id}")]
        public Board DeleteBoard(string id)
        {
            try
            {
                var userId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
                Board board = _boardRepo.GetBoardById(userId, id);
                if (board.UserId == userId)
                {
                    return _boardRepo.DeleteBoard(id);
                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}