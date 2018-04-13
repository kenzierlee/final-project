using System;
using System.Collections.Generic;
using System.Data;
using keepr.Models;
using Dapper;

namespace keepr.Repositories
{
    public class BoardRepository
    {
        private readonly IDbConnection _db;
        public BoardRepository(IDbConnection db)
        {
            _db = db;
        }
        public Board GetBoardById(string userId, string id)
        {
            try
            {
                Board board = _db.QueryFirstOrDefault<Board>(@"
                    SELECT * FROM boards WHERE id = @id
                    ", new { id = id });
                if (board.UserId == userId)
                {
                    return board;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return null;
            }
        }
        public IEnumerable<Board> getBoardsByUserId(string userId)
        {
            return _db.Query<Board>(@"
            SELECT 
                ab.id,
                ab.name,
                ab.description,
                ab.userId
            FROM boards ab
            WHERE ab.userId = @userId;
            ", new { userId = userId });
        }
        public Board AddBoard(Board boardData)
        {
            Guid g;
            g = Guid.NewGuid();
            string id = g.ToString();
            Board board = new Board()
            {
                Id = id,
                Name = boardData.Name,
                Description = boardData.Description,
                UserId = boardData.UserId
            };
            var success = _db.Execute(@"
            INSERT INTO boards(
                id,
                name,
                description,
                userId
            ) VALUES(@Id, @Name, @Description, @UserId)", board);
            if (success < 1)
            {
                throw new Exception("Invalid Board");
            }
            else
            {
                return new Board()
                {
                    Id = board.Id,
                    Name = board.Name,
                    Description = board.Description,
                    UserId = board.UserId
                };
            }
        }
        public Board UpdateBoard(Board board, Board boardData)
        {
            var i = _db.Execute(@"
            UPDATE boards SET
                name = @Name,
                description = @Description
            WHERE id = @Id", boardData);
            if (i > 0)
            {
                return board;
            }
            else
            {
                return null;
            }
        }
        public Board DeleteBoard(string id)
        {
            return _db.QueryFirstOrDefault<Board>(@"
                DELETE FROM boards WHERE id = @id
                ", new { id = id });
        }
    }
}