using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DistributionStamping.StampingChain.Test
{
	[TestClass]
	public class StampingPeerSpec
	{
		[TestMethod]
		public void should_throw_exception_of_invalid_previous_block_hash()
		{
			var peer = new StampPeer(Guid.NewGuid());
			var stamp = DateTime.Now;
			peer.GenerateBlock("001", stamp);
			var block = new StampBlock(Guid.NewGuid(), null, "001", stamp);
			try
			{
				peer.AddBlock(block);
			}
			catch (Exception e)
			{
				Assert.AreEqual("Invalid previous block hash", e.Message);
			}
		}

		[TestMethod]
		public void should_throw_exception_of_duplicate_block_in_chain()
		{
			var peer = new StampPeer(Guid.NewGuid());
			var stamp = DateTime.Now;
			var block = peer.GenerateBlock("001", stamp);
			peer.AddBlock(block);
			try
			{
				peer.AddBlock(block);
			}
			catch (Exception e)
			{
				Assert.AreEqual("Invalid previous block hash", e.Message);
			}
		}

		[TestMethod]
		public void should_clone_peer()
		{
			var peer1 = new StampPeer(Guid.NewGuid());
			var block1 = peer1.GenerateBlock("001", DateTime.Now);
			peer1.AddBlock(block1);
			var peer2 = peer1.Clone();
			var block2 = peer1.GenerateBlock("002", DateTime.Now);
			peer1.AddBlock(block2);
			peer2.AddBlock(block2);
		}

		[TestMethod]
		public void should_calculate_peer_hash()
		{
			var peer1 = new StampPeer(Guid.NewGuid());
			var block1 = peer1.GenerateBlock("001", DateTime.Now);
			peer1.AddBlock(block1);
			var peer2 = peer1.Clone();
			var block2 = peer1.GenerateBlock("002", DateTime.Now);
			peer1.AddBlock(block2);
			peer2.AddBlock(block2);

			Assert.AreEqual(peer1.PeerHash, peer2.PeerHash);
		}
	}
}
