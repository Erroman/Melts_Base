using Melts_Base.OracleModels;
using Melts_Base.SybaseModels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Melts_Base.BackgroundSync
{
    internal sealed class FakeSybaseMeltSource : ISybaseMeltSource
    {
        private int _pollCounter;

        public Task<IReadOnlyList<SybaseMelt>> ReadAsync(CancellationToken cancellationToken)
        {
            _pollCounter++;
            var now = DateTime.Now;
            var melts = new List<SybaseMelt>
            {
                new SybaseMelt
                {
                    Me_id = "fake-1",
                    Eq_id = "31",
                    Me_num = "A-1001",
                    Me_beg = now.Date.AddHours(7),
                    Me_end = now.Date.AddHours(10),
                    Me_splav = "VT6",
                    Sp_name = "VT6 Titanium",
                    Me_mould = "K-101",
                    Me_del = "120",
                    Me_ukaz = "ИЛ",
                    Me_kont = "CNT-01",
                    Me_pril = "APP-1",
                    Me_nazn = "Aircraft",
                    Me_diam = "450",
                    Me_weight = (980 + _pollCounter).ToString(),
                    Me_pos = "P1",
                    Sp_id = "VT6"
                },
                new SybaseMelt
                {
                    Me_id = "fake-2",
                    Eq_id = "31",
                    Me_num = "A-1002",
                    Me_beg = now.Date.AddHours(11),
                    Me_end = null,
                    Me_splav = "VT14",
                    Sp_name = "VT14 Titanium",
                    Me_mould = "K-102",
                    Me_del = "140",
                    Me_ukaz = "УиС",
                    Me_kont = "CNT-02",
                    Me_pril = "APP-2",
                    Me_nazn = "Medical",
                    Me_diam = "500",
                    Me_weight = "1020",
                    Me_pos = "P2",
                    Sp_id = "VT14"
                }
            };

            if (_pollCounter % 3 == 0)
            {
                melts.Add(new SybaseMelt
                {
                    Me_id = $"fake-3-{_pollCounter}",
                    Eq_id = "31",
                    Me_num = "A-1003",
                    Me_beg = now,
                    Me_end = null,
                    Me_splav = "VT1",
                    Sp_name = "VT1 Pure",
                    Me_mould = "K-103",
                    Me_del = "110",
                    Me_ukaz = "ШН",
                    Me_kont = "CNT-03",
                    Me_pril = "APP-3",
                    Me_nazn = "Test",
                    Me_diam = "420",
                    Me_weight = "900",
                    Me_pos = "P3",
                    Sp_id = "VT1"
                });
            }

            return Task.FromResult<IReadOnlyList<SybaseMelt>>(melts);
        }
    }

    internal sealed class FakeOracleMeltSource : IOracleMeltSource
    {
        public Task<IReadOnlyList<OracleMelt>> ReadAsync(CancellationToken cancellationToken)
        {
            var now = DateTime.Now;
            var melts = new List<OracleMelt>
            {
                new OracleMelt
                {
                    Nplav = "A-1001",
                    Npech = "31",
                    Ins = "INS-001",
                    Tek = "TEK-001",
                    Pereplav = "1",
                    OkonchPereplav = "Y",
                    Poz = "POZ-1",
                    PozNaim = "Rotor",
                    DateZap = now.Date.AddHours(7),
                    DateClose = now.Date.AddHours(10)
                },
                new OracleMelt
                {
                    Nplav = "A-1002",
                    Npech = "31",
                    Ins = "INS-002",
                    Tek = "TEK-002",
                    Pereplav = "0",
                    OkonchPereplav = "N",
                    Poz = "POZ-2",
                    PozNaim = "Compressor",
                    DateZap = now.Date.AddHours(11),
                    DateClose = null
                }
            };

            return Task.FromResult<IReadOnlyList<OracleMelt>>(melts);
        }
    }
}