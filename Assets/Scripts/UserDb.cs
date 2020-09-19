using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DataBank{
	public class UserDb : SqliteHelper {
		private const String Tag = "UserDb:\t";

		private const String TABLE_NAME = "User";
		private const String KEY_ID = "id";
		private const String KEY_NAME = "name";
		private const String KEY_STATUS = "status";

		public UserDb() : base()
		{
			IDbCommand dbcmd = getDbCommand();
			dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
				KEY_ID + " INTEGER PRIMARY KEY, " + KEY_NAME + " TEXT, " + KEY_STATUS + " TEXT )";
			dbcmd.ExecuteNonQuery();
		}

		public void addData(UserEntity user)
		{
			IDbCommand dbcmd = getDbCommand();
			dbcmd.CommandText = "INSERT INTO " + TABLE_NAME + " ( " + KEY_NAME  + ", " + KEY_STATUS + " ) "
				+ "VALUES ( '" + user._name + "', '" + user._status + "' )";
			dbcmd.ExecuteNonQuery();
		}

		public void nonActivateAllUsers()
		{
			IDbCommand dbcmd = getDbCommand();
			dbcmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + KEY_STATUS  + " = 'inactive'";
			dbcmd.ExecuteNonQuery();
		}

		public void activateUser(string id)
		{
			Debug.Log ("Activate User : " + id);
			IDbCommand dbcmd = getDbCommand();
			dbcmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + KEY_STATUS  + " = 'active' WHERE " + KEY_ID + " = '" + id + "'";
			dbcmd.ExecuteNonQuery();
		}

		public UserEntity getDataById(int id)
		{
			Debug.Log(Tag + "Getting User: " + id);

			IDbCommand dbcmd = getDbCommand();
			dbcmd.CommandText =
				"SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ID + " = '" + id + "'";

			System.Data.IDataReader reader = dbcmd.ExecuteReader();

			int fieldCount = reader.FieldCount;
			UserEntity user = new UserEntity();
			while (reader.Read())
			{
				user.setId (reader [0].ToString ());
				user.setName (reader [1].ToString ());
				user.setStatus (reader [2].ToString ());
			}

			Debug.Log(Tag + "Getting User : " + user._id);

			return user;
		}

		public UserEntity getActiveUser()
		{
			IDbCommand dbcmd = getDbCommand();
			dbcmd.CommandText =
				"SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_STATUS + " = 'active'";
			
			System.Data.IDataReader reader = dbcmd.ExecuteReader();

			int fieldCount = reader.FieldCount;
			UserEntity user = new UserEntity();
			while (reader.Read())
			{
				user.setId (reader [0].ToString ());
				user.setName (reader [1].ToString ());
				user.setStatus (reader [2].ToString ());
			}

			Debug.Log(Tag + "Getting Active User " + user._id);

			return user;
		}

		public void deleteDataById(string id)
		{
			Debug.Log(Tag + "Deleting User: " + id);

			IDbCommand dbcmd = getDbCommand();
			dbcmd.CommandText =
				"DELETE FROM " + TABLE_NAME + " WHERE " + KEY_ID + " = '" + id + "'";
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

		public List<UserEntity> getAllUser()
		{
			IDbCommand dbcmd = getDbCommand();
			dbcmd.CommandText =
				"SELECT * FROM " + TABLE_NAME + " ORDER BY " + KEY_STATUS + " ASC";

			System.Data.IDataReader reader = dbcmd.ExecuteReader();

			int fieldCount = reader.FieldCount;
			List<UserEntity> users = new List<UserEntity>();
			while (reader.Read())
			{
				UserEntity user = new UserEntity(	reader[0].ToString(), 
					reader[1].ToString(), 
					reader[2].ToString());

				Debug.Log(" id: " + user._id);
				users.Add(user);
			}

			return users;
		}
	}
}