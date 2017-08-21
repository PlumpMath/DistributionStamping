using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DistributionStamping.StampingChain
{
	public class StampBlock
	{
		public Guid BlockGuid {
			private set;
			get;
		}
		public string PreviousHash {
			private set;
			get;
		}
		public DateTime CreatedAt {
			private set;
			get;
		}
		public string UserCode
		{
			private set;
			get;
		}
		public DateTime StampAt
		{
			private set;
			get;
		}
		public string Note
		{
			private set;
			get;
		}
		
		public string BlockHash
		{
			get
			{
				var data = String.Concat(BlockGuid.ToString(), PreviousHash, CreatedAt.ToUniversalTime(), UserCode, StampAt.ToUniversalTime(), Note);
				var content = Encoding.ASCII.GetBytes(data);
				var hasher = SHA256Managed.Create();
				byte[] hash = hasher.ComputeHash(content);

				StringBuilder result = new StringBuilder();
				for (int i = 0; i < hash.Length; i++)
				{
					result.Append(hash[i].ToString("X2"));
				}
				return result.ToString();
			}
		}
		public StampBlock(Guid blockGuid, string previousHash, string userCode, DateTime stampAt, string note = null)
		{
			BlockGuid = blockGuid;
			PreviousHash = previousHash;
			CreatedAt = DateTime.Now;
			UserCode = userCode;
			StampAt = stampAt;
			if(!String.IsNullOrWhiteSpace(note))
				Note = note;
		}
	}
}
