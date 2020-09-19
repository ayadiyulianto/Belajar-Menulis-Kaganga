using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DataBank{
	public class HistoryDb : SqliteHelper {
		private const String Tag = "HistoryDb:\t";

		private const String TABLE_NAME = "History";
		private const String KEY_ID = "id";
		private const String KEY_USERID = "userId";
		private const String KEY_TYPE = "type";
		private const String KEY_DATETIME = "datetime";
		private const String KEY_SCORE = "score";

		public HistoryDb() : base()
		{
			IDbCommand dbcmd = getDbCommand();
			dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
				KEY_ID + " INTEGER PRIMARY KEY, " + KEY_USERID + " INTEGER, " + KEY_TYPE + " TEXT," +
				KEY_DATETIME + " DATETIME DEFAULT CURRENT_TIMESTAMP, " + KEY_SCORE + " INTEGER )";
			dbcmd.ExecuteNonQuery();
		}

		public void addData(HistoryEntity history)
		{
			IDbCommand dbcmd = getDbCommand();
			dbcmd.CommandText = "INSERT INTO " + TABLE_NAME
				+ " ( " + KEY_USERID  + ", " + KEY_TYPE + ", "+ KEY_SCORE + " ) "
				+ "VALUES ( '" + history._userId + "', '" + history._type + "', '" + history._score + "' )";
			dbcmd.ExecuteNonQuery();
		}

		public List<HistoryEntity> getDataByUserId(string str)
		{
			Debug.Log(Tag + "Getting User: " + str);

			IDbCommand dbcmd = getDbCommand();
			dbcmd.CommandText =
				"SELECT History.id, User.name, type, DATE(datetime), score FROM History JOIN User ON User.id = History.userId WHERE History.userId = '" + str + "'";

			System.Data.IDataReader reader = dbcmd.ExecuteReader();

			int fieldCount = reader.FieldCount;
			List<HistoryEntity> histories = new List<HistoryEntity>();
			while (reader.Read())
			{
				HistoryEntity history = new HistoryEntity(	reader[0].ToString(), 
					reader[1].ToString(), 
					reader[2].ToString(), 
					reader[3].ToString(), 
					reader[4].ToString());

				Debug.Log(" id: " + history._id);
				histories.Add(history);
			}

			return histories;
		}

		public void deleteDataById(int id)
		{
			Debug.Log(Tag + "Deleting History: " + id);

			IDbCommand dbcmd = getDbCommand();
			dbcmd.CommandText =
				"DELETE FROM " + TABLE_NAME + " WHERE " + KEY_ID + " = '" + id + "'";
			dbcmd.ExecuteNonQuery();
		}

		public void deleteDataByUserId(string id)
		{
			Debug.Log(Tag + "Deleting User: " + id);

			IDbCommand dbcmd = getDbCommand();
			dbcmd.CommandText =
				"DELETE FROM " + TABLE_NAME + " WHERE " + KEY_USERID + " = '" + id + "'";
			dbcmd.ExecuteNonQuery();
		}

		public override void deleteAllData()
		{
			Debug.Log(Tag + "Deleting Table");

			base.deleteAllData(TABLE_NAME);
		}

		public override IDataReader getAllData()
		{
			return base.getAllData(TABLE_NAME);
		}
	}
}