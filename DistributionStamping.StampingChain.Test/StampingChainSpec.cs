using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DistributionStamping.StampingChain.Test
{
	[TestClass]
	public class StampingChainSpec
	{
		[TestMethod]
		public void should_compute_block_hash()
		{
			var genesisBlock = new StampBlock(Guid.Empty, null, null, new DateTime(2017, 8, 20));
			var firstBlock = new StampBlock(Guid.NewGuid(), genesisBlock.BlockHash, "0001", DateTime.Now);
			Assert.AreEqual(genesisBlock.BlockHash, firstBlock.PreviousHash);
		}
	}
}
