using System;
using System.Collections.Generic;
using System.Data;
using keepr.Models;
using Dapper;

namespace keepr.Repositories
{
    public class PinRepository
    {
        private readonly IDbConnection _db;
        public PinRepository(IDbConnection db)
        {
            _db = db;
        }
        public IEnumerable<Pin> GetPins()
        {
            return _db.Query<Pin>("SELECT * FROM pins WHERE public = 1;");
        }
        public IEnumerable<Pin> GetBoardPins(string boardId)
        {
            return _db.Query<Pin>(@"
            SELECT
                bp.pinId,
                bp.boardId,
                bp.id,
                p.name,
                p.description,
                p.imgUrl,
                p.href,
                p.userId,
                p.views,
                p.saves,
                p.public
            FROM boardpins bp
            JOIN pins p ON p.id = bp.pinId
            WHERE boardId = @id;
            ", new { id = boardId });
        }
        public Pin GetPinById(string userId, string pinId)
        {
            Pin pin = _db.QueryFirstOrDefault<Pin>(@"
                SELECT * FROM pins WHERE id = @pinId
                ", new { id = pinId });
            if (pin.UserId == userId)
            {
                return pin;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<Pin> GetUserPins(string userId)
        {
            return _db.Query<Pin>(@"
            SELECT * FROM pins WHERE userId = @userId;
            ", new { userId = userId });
        }
        public Pin AddPin(Pin pinData)
        {
            Guid g;
            g = Guid.NewGuid();
            string id = g.ToString();
            Pin pin = new Pin()
            {
                Id = id,
                Name = pinData.Name,
                Description = pinData.Description,
                ImgUrl = pinData.ImgUrl,
                Href = pinData.Href,
                Views = pinData.Views,
                Saves = pinData.Saves,
                Public = pinData.Public,
                UserId = pinData.UserId
            };
            var success = _db.Execute(@"
            INSERT INTO pins(
                id,
                name,
                description,
                imgUrl,
                href,
                views,
                saves,
                public,
                userId
            ) VALUES(@Id, @Name, @Description, @ImgUrl, @Href, @Views, @Saves, @Public, @UserId)", pin);
            if (success < 1)
            {
                throw new Exception("Invalid Pin");
            }
            else
            {
                return new Pin()
                {
                    Id = pin.Id,
                    Name = pin.Name,
                    Description = pin.Description,
                    ImgUrl = pin.ImgUrl,
                    Href = pin.Href,
                    Views = pin.Views,
                    Saves = pin.Saves,
                    Public = pin.Public,
                    UserId = pin.UserId
                };
            }
        }
        public BoardPin SavePin(BoardPin boardPinData)
        {
            Guid g;
            g = Guid.NewGuid();
            string id = g.ToString();
            BoardPin boardPin = new BoardPin()
            {
                Id = id,
                BoardId = boardPinData.BoardId,
                PinId = boardPinData.PinId,
                UserId = boardPinData.UserId
            };
            var success = _db.Execute(@"
            INSERT INTO boardpins(
                id,
                boardId,
                pinId,
                userId
            ) VALUES(@Id, @BoardId, @PinId, @UserId)", boardPin);
            if (success < 1)
            {
                throw new Exception("Invalid BoardPin");
            }
            else
            {
                return new BoardPin()
                {
                    Id = boardPin.Id,
                    BoardId = boardPin.BoardId,
                    PinId = boardPin.Id,
                    UserId = boardPin.UserId
                };
            }
        }
        public Pin EditPin(Pin pin, Pin pinData)
        {
            var i = _db.Execute(@"
            UPDATE pins SET
                imgUrl = @ImgUrl,
                href = @Href,
                name = @Name,
                description = @Description
            WHERE id = @Id", pinData);
            if (i > 0)
            {
                return pin;
            }
            else
            {
                return null;
            }
        }
        public string UpdateViews(string id)
        {
            var i = _db.Execute(@"
            UPDATE pins SET
                views = views+1
            WHERE id = @id
            ", new { id = id });
            if (i > 0)
            {
                return id;
            }
            else
            {
                return null;
            }
        }
        public string UpdateSaves(string id)
        {
            var i = _db.Execute(@"
            UPDATE pins SET
                saves = saves+1
            WHERE id = @id
            ", new { id = id });
            if (i > 0)
            {
                return id;
            }
            else
            {
                return null;
            }
        }
        public string DeletePin(string id)
        {
            var i = _db.Execute(@"
                DELETE FROM pins WHERE id = @id
                ", new { id = id });
            if (i > 0)
            {
                return id;
            }
            else
            {
                return null;
            }
        }
        public string DeleteBoardPin(string id)
        {
            var i = _db.Execute(@"
                DELETE FROM boardpins WHERE id = @id
                ", new { id = id });
            if (i > 0)
            {
                return id;
            }
            else
            {
                return null;
            }
        }
    }
}