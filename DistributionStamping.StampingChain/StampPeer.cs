using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DistributionStamping.StampingChain
{
	public class StampPeer
	{
		public Guid PeerGuid
		{
			private set;
			get;
		}
		private StampBlock _genesisBlock;
		public StampBlock LatestBlock
		{
			private set;
			get;
		}
		public Dictionary<string, StampBlock> Blocks
		{
			private set;
			get;
		}

		public StampPeer(Guid peerGuid)
		{
			PeerGuid = peerGuid;
			_genesisBlock = new StampBlock(Guid.Empty, null, null, DateTime.Now);
			Blocks = new Dictionary<string, StampBlock>() { { _genesisBlock.BlockHash, _genesisBlock } };
			LatestBlock = _genesisBlock;
		}

		public String PeerHash
		{
			get
			{
				var block = LatestBlock;
				var hashBlocks = String.Empty;
				while (block.PreviousHash != null)
				{
					hashBlocks = String.Concat(hashBlocks, block.BlockHash);
					block = Blocks[block.PreviousHash];
				}
				return CalculateHash(hashBlocks);
			}
		}

		public StampBlock GenerateBlock(string userCode, DateTime stampAt, string note = null)
		{
			return new StampBlock(Guid.NewGuid(), LatestBlock.BlockHash, userCode, stampAt, note);
		}

		public void AddBlock(StampBlock block)
		{
			if (LatestBlock.BlockHash != block.PreviousHash)
				throw new Exception("Invalid previous block hash");
			var blockHash = block.BlockHash;
			if (CalculateBlockHash(block) != blockHash)
				throw new Exception("Invalid new block hash");
			if(Blocks.ContainsKey(blockHash))
				throw new Exception("Duplicate block in chain");
			Blocks.Add(blockHash, block);
			LatestBlock = block;
		}

		public StampPeer Clone()
		{
			var peer = new StampPeer(Guid.NewGuid());
			foreach (var b in Blocks.Values)
			{
				if(b.BlockGuid != Guid.Empty)
					peer.AddBlock(b);
			}
			return peer;
		}

		private String CalculateBlockHash(StampBlock block)
		{
			var data = String.Concat(block.BlockGuid.ToString(), block.PreviousHash, block.CreatedAt.ToUniversalTime(), block.UserCode, block.StampAt.ToUniversalTime(), block.Note);
			return CalculateHash(data);
		}

		private String CalculateHash(string data)
		{
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
}
