using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBank
{
	public class HistoryEntity {

		public string _id, _userId, _type, _datetime, _score;

		public HistoryEntity(string id, string userId, string type, string datetime, string score)
		{
			_id = id;
			_userId = userId;
			_type = type;
			_datetime = datetime;
			_score = score;
		}

		public HistoryEntity(string userId, string type, string score)
		{
			_id = "0";
			_userId = userId;
			_type = type;
			_datetime = "";
			_score = score;
		}

	}
}