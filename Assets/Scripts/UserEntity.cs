using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBank
{
	public class UserEntity {

		public string _id, _name, _status;

		public UserEntity(){
		}

		public UserEntity(string id, string name, string status)
		{
			_id = id;
			_name = name;
			_status = status;
		}

		public UserEntity(string name)
		{
			_id = "0";
			_name = name;
			_status = "inactive";
		}

		public void setId(string id){
			_id = id;
		}

		public void setName(string name){
			_name = name;
		}

		public void setStatus(string status){
			_status = status;
		}
	}
}