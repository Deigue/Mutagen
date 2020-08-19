﻿using Alphaleonis.Win32.Filesystem;
using Noggog.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Mutagen.Bethesda.UnitTests
{
    public class LoadOrder_Tests
    {
        [Fact]
        public void AlignToTimestamps_Typical()
        {
            var lo = new List<(ModKey ModKey, DateTime Write)>
            {
                (Utility.ModKey4, new DateTime(2020, 8, 8, 10, 11, 0)),
                (Utility.ModKey, new DateTime(2020, 8, 8, 10, 8, 0)),
                (Utility.ModKey3, new DateTime(2020, 8, 8, 10, 10, 0)),
                (Utility.ModKey2, new DateTime(2020, 8, 8, 10, 9, 0)),
            };
            var results = LoadOrder.AlignToTimestamps(lo)
                .ToList();
            Assert.Equal(Utility.ModKey, results[0]);
            Assert.Equal(Utility.ModKey2, results[1]);
            Assert.Equal(Utility.ModKey3, results[2]);
            Assert.Equal(Utility.ModKey4, results[3]);
        }

        [Fact]
        public void AlignToTimestamps_SameTimestamps()
        {
            var lo = new List<(ModKey ModKey, DateTime Write)>
            {
                (Utility.ModKey4, new DateTime(2020, 8, 8, 10, 11, 0)),
                (Utility.ModKey, new DateTime(2020, 8, 8, 10, 8, 0)),
                (Utility.ModKey3, new DateTime(2020, 8, 8, 10, 10, 0)),
                (Utility.ModKey2, new DateTime(2020, 8, 8, 10, 11, 0)),
            };
            var results = LoadOrder.AlignToTimestamps(lo)
                .ToList();
            Assert.Equal(Utility.ModKey, results[0]);
            Assert.Equal(Utility.ModKey3, results[1]);
            Assert.Equal(Utility.ModKey4, results[2]);
            Assert.Equal(Utility.ModKey2, results[3]);
        }

        [Fact]
        public void AddImplicitMods()
        {
            using var tmpFolder = new TempFolder(Path.Combine(Utility.TempFolderPath, "AddImplicitMods"));
            File.WriteAllText(Path.Combine(tmpFolder.Dir.Path, Utility.Skyrim.FileName), "TEST");
            File.WriteAllText(Path.Combine(tmpFolder.Dir.Path, Utility.Dawnguard.FileName), "TEST");
            File.WriteAllText(Path.Combine(tmpFolder.Dir.Path, Utility.Dragonborn.FileName), "TEST");
            File.WriteAllText(Path.Combine(tmpFolder.Dir.Path, Utility.Update.FileName), "TEST");
            List<ModKey> loadOrder = new List<ModKey>()
            {
                Utility.ModKey,
                Utility.Dawnguard,
                Utility.ModKey2
            };
            LoadOrder.AddImplicitMods(GameRelease.SkyrimSE, tmpFolder.Dir, loadOrder);
            Assert.Equal(6, loadOrder.Count);
            Assert.Equal(Utility.Skyrim, loadOrder[0]);
            Assert.Equal(Utility.Update, loadOrder[1]);
            Assert.Equal(Utility.Dragonborn, loadOrder[2]);
            Assert.Equal(Utility.ModKey, loadOrder[3]);
            Assert.Equal(Utility.Dawnguard, loadOrder[4]);
            Assert.Equal(Utility.ModKey2, loadOrder[5]);
        }
    }
}