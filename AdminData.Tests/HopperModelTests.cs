﻿using AdminData.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace AdminData.Tests
{
    [TestClass]
    public class HopperModelTests
    {
        [TestMethod]
        public void ParseLine()
        {
            var expected = ExpectedFirstRecord();

            var line = "2012-12-05T17:00:00.0000000-08:00	186273	139562	106	0.057";
            var reader = new HopperCountReader(string.Empty);
            var actual = reader.ReadLine(line);
            actual.ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        public void ParseFile()
        {
            var path = "c:\\Code\\AdminData\\AdminData\\App_Data\\data.txt";
            var reader = new HopperCountReader(path);
            var hopperCounts = reader.GetHopperCounts().ToArray();
            hopperCounts.Should().NotBeNull();
            hopperCounts.Should().NotBeEmpty();
            hopperCounts.First().ShouldBeEquivalentTo(ExpectedFirstRecord());
            hopperCounts.Count().Should().Be(954);
        }

        private static HopperCount ExpectedFirstRecord()
        {
            var expected = new HopperCount
            {
                Date = DateTime.Parse("2012-12-05T17:00:00.0000000-08:00"),
                FallbackHoppers = 106,
                FallbackRatio = .057M,
                Sessions = 139562,
                TotalCount = 186273
            };
            return expected;
        }
    }
}
