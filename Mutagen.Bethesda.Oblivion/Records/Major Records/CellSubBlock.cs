using Loqui.Internal;
using Loqui.Xml;
using Mutagen.Bethesda.Binary;
using Mutagen.Bethesda.Oblivion.Internals;
using Noggog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mutagen.Bethesda.Oblivion
{
    public partial class CellSubBlock
    {
        public static CellSubBlock.TranslationMask duplicateMask = new CellSubBlock.TranslationMask(true)
        {
            Cells = false
        };

        public object Duplicate(Func<FormKey> getNextFormKey, IList<(IMajorRecordCommon Record, FormKey OriginalFormKey)>? duplicatedRecordTracker = null)
        {
            var ret = new CellSubBlock();
            ret.DeepCopyIn(this, duplicateMask);
            ret.Cells = this.Cells.Select(i => (Cell)i.Duplicate(getNextFormKey, duplicatedRecordTracker)).ToExtendedList();
            return ret;
        }
    }

    namespace Internals
    {
        partial class CellSubBlockBinaryOverlay
        {
            public IReadOnlyList<ICellGetter> Cells { get; private set; } = ListExt.Empty<ICellGetter>();

            partial void CellsCustomParse(OverlayStream stream, long finalPos, int offset, RecordType type, int? lastParsed)
            {
                this.Cells = BinaryOverlayList.FactoryByArray<CellBinaryOverlay>(
                    mem: stream.RemainingMemory,
                    package: _package,
                    recordTypeConverter: null,
                    getter: (s, p, recConv) => CellBinaryOverlay.CellFactory(new OverlayStream(s, p), p, recConv),
                    locs: CellBinaryOverlay.ParseRecordLocations(
                        stream: stream,
                        package: _package));
            }
        }
    }
}
