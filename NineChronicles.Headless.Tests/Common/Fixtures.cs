using System;
using System.IO;
using Lib9c.DevExtensions.Model;
using Lib9c.Model.Order;
using Lib9c.Tests;
using Libplanet;
using Libplanet.Assets;
using Libplanet.Crypto;
using Nekoyume.Model.Item;
using Nekoyume.Model.State;

namespace NineChronicles.Headless.Tests
{
    public static class Fixtures
    {
        public static readonly PrivateKey UserPrivateKey =
            new PrivateKey(ByteUtil.ParseHex("b934cb79757b1dec9f89caa01c4b791a6de6937dbecdc102fbdca217156cc2f5"));

        public static readonly Address MinerAddress = new PrivateKey().PublicKey.ToAddress();

        public static readonly Address UserAddress = UserPrivateKey.PublicKey.ToAddress();

        public static readonly Address AvatarAddress = new Address("983c3Fbfe8243a0e36D55C6C1aE26A7c8Bb6CBd4");

        public static readonly Address StakeStateAddress = StakeState.DeriveAddress(UserAddress);

        public static readonly TableSheets TableSheetsFX =
            new TableSheets(
                TableSheetsImporter.ImportSheets(
                    Path.Join("..", "..", "..", "..", "Lib9c", "Lib9c", "TableCSV")));

        public static readonly AvatarState AvatarStateFX = new AvatarState(
            AvatarAddress,
            UserAddress,
            0,
            TableSheetsFX.GetAvatarSheets(),
            new GameConfigState(),
            new Address(),
            "avatar_state_fx"
        );

        public static readonly AgentState AgentStateFx = new AgentState(UserAddress)
        {
            avatarAddresses = { [2] = AvatarAddress },
        };

        public static readonly Currency CurrencyFX = new Currency("NCG", 2, minter: null);

        public static ShopState ShopStateFX()
        {
            var shopState = new ShopState();
            for (var index = 0; index < TableSheetsFX.EquipmentItemSheet.OrderedList.Count; index++)
            {
                var row = TableSheetsFX.EquipmentItemSheet.OrderedList[index];
                var equipment = ItemFactory.CreateItemUsable(row, default, 0);
                var shopItem = new ShopItem(UserAddress, AvatarAddress, Guid.NewGuid(), index * CurrencyFX, equipment);
                shopState.Register(shopItem);
            }

            for (var i = 0; i < TableSheetsFX.CostumeItemSheet.OrderedList.Count; i++)
            {
                var row = TableSheetsFX.CostumeItemSheet.OrderedList[i];
                var equipment = ItemFactory.CreateCostume(row, default);
                var shopItem = new ShopItem(UserAddress, AvatarAddress, Guid.NewGuid(), i * CurrencyFX, equipment);
                shopState.Register(shopItem);
            }
            return shopState;
        }

        public static ShardedShopStateV2 ShardedWeapon0ShopStateV2FX()
        {
            Address shardedWeapon0ShopStateV2Address = ShardedShopStateV2.DeriveAddress(ItemSubType.Weapon, "0");
            var shardedShopV2State = new ShardedShopStateV2(shardedWeapon0ShopStateV2Address);

            var orderDigest = new OrderDigest(
                default,
                1,
                3,
                new Guid("F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4"),
                new Guid("45082f35-699c-41f0-9332-9143966933a3"),
                new FungibleAssetValue(new Currency("NCG", 2, minter: null), 1, 0),
                0,
                0,
                10110000,
                1
            );
            var orderDigest2 = new OrderDigest(
                default,
                2,
                4,
                new Guid("936DA01F-9ABD-4d9d-80C7-02AF85C822A8"),
                new Guid("dae32f1b-6b43-4bdb-933e-fd51d003283e"),
                new FungibleAssetValue(new Currency("NCG", 2, minter: null), 2, 0),
                0,
                0,
                10110000,
                1
            );

            shardedShopV2State.Add(orderDigest, 1);
            shardedShopV2State.Add(orderDigest2, 2);

            return shardedShopV2State;
        }
    }
}
