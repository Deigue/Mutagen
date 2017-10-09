﻿using Mutagen.Binary;
using Mutagen.Internals;
using Noggog.Notifying;
using System;
using System.IO;

namespace Mutagen
{
    public partial class GameSetting
    {
        protected abstract char TriggerChar { get; }

        partial void CustomCtor()
        {
            this.EditorID_Property.Subscribe(CorrectEDID, fireInitial: false);
        }

        private void CorrectEDID(Change<string> change)
        {
            if (change.New.Length == 0)
            {
                this.EditorID = string.Empty + this.TriggerChar;
            }
            else if (!this.TriggerChar.Equals(change.New[0]))
            {
                this.EditorID = this.TriggerChar + change.New;
            }
        }

        public static (GameSetting Object, GameSetting_ErrorMask ErrorMask) Create_Binary(
            BinaryReader reader,
            bool doMasks)
        {
            var initialPos = reader.BaseStream.Position;
            reader.BaseStream.Position += 20;
            if (!MajorRecord_Registration.EDID_HEADER.Equals(HeaderTranslation.GetNextSubRecordType(reader, out var contentLength)))
            {
                throw new ArgumentException($"EDID was not located in expected position: {reader.BaseStream.Position}");
            }
            var edid = StringBinaryTranslation.Instance.Parse(
                reader,
                contentLength,
                doMasks,
                out var err);
            if (err != null)
            {
                throw new ArgumentException($"Error parsing EDID: {err}");
            }
            if (edid.Failed || edid.Value.Length == 0)
            {
                throw new ArgumentException("No EDID parsed.");
            }
            reader.BaseStream.Position = initialPos;
            switch (edid.Value[0])
            {
                case GameSettingInt.TRIGGER_CHAR:
                    return GameSettingInt.Create_Binary(reader, doMasks);
                case GameSettingString.TRIGGER_CHAR:
                    return GameSettingString.Create_Binary(reader, doMasks);
                case GameSettingFloat.TRIGGER_CHAR:
                    return GameSettingFloat.Create_Binary(reader, doMasks);
                default:
                    throw new ArgumentException($"Unknown game setting type: {edid.Value[0]}");
            }
        }
    }
}