using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookNest.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixDecimalPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("0ad20032-5475-4885-8ed8-e16082db1470"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("4568668b-0bab-4a0e-a09d-1782a0c8b3fd"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("2bada63b-74c8-4d07-98cb-b7403af35183"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("6c7799c4-fe32-418d-9a01-b6f9e732e48a"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("f1c43376-6547-4601-8a9e-a258c200a0b3"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("1a8d90be-d853-4aee-9412-a08c0526f03f"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("5eec2c35-8c32-49e6-af2b-5c2d482041d1"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("6b87d71b-586f-4fee-a049-dba00f2f8bc8"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("774a3b61-bd46-480f-baf6-1afb2871736a"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("778209d9-2e72-46ab-a63d-aa0da95a2b84"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("84d896be-2f5a-454b-a93e-9719df3cf516"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d355381b-b235-492a-a812-d6f2bb4d51eb"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("eab18715-670c-439b-82dc-5fb36eef2a54"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("ed47737e-c460-4a0c-bd3e-a5cacc9ea860"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("d48c3ec1-0c05-426e-abc6-0732a11bbbab"));

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Created", "DisplayOrder", "Name", "Updated" },
                values: new object[,]
                {
                    { new Guid("08366134-aa61-4b97-8f9b-1aa17d66b242"), new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(8351), 1, "Scifi", new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(8383) },
                    { new Guid("9f5eeeac-95fc-47ef-99fc-b775431e978b"), new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(8899), 2, "Action", new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(8917) },
                    { new Guid("be776611-b2de-4e6c-8a3c-77eac2d8c0ce"), new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(8948), 3, "History", new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(8948) }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "City", "Created", "Name", "PhoneNumber", "PostalCode", "State", "StreetAddress", "Updated" },
                values: new object[,]
                {
                    { new Guid("67daf9c6-7b27-4c3f-b9fb-0fa4f63e200f"), "Vid City", new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9632), "Vivid Books", "7779990000", "66666", "IL", "999 Vid St", new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9633) },
                    { new Guid("6fb4b932-5c54-4bf5-9e02-2007a0cbcddb"), "Lala land", new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9637), "Readers Club", "1113335555", "99999", "NY", "999 Main St", new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9637) },
                    { new Guid("d3c3e1cb-7146-436b-973d-7a9e17aad693"), "Tech City", new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9612), "Tech Solution", "6669990000", "12121", "IL", "123 Tech St", new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9616) }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "CategoryId", "Created", "Description", "ISBN", "ImageUrl", "ListPrice", "Price", "Price100", "Price50", "Title", "Updated" },
                values: new object[,]
                {
                    { new Guid("29bab671-2efd-4068-9cdd-2adb8e7a4618"), "Corey, James S. A.", new Guid("08366134-aa61-4b97-8f9b-1aa17d66b242"), new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9853), "<p>In the thousand-sun network of humanity's expansion, new colony worlds are struggling to find their way. Every new planet lives on a knife edge between collapse and wonder, and the crew of the aging gunship Rocinante have their hands more than full keeping the fragile peace.<br><br>In the vast space between Earth and Jupiter, the inner planets and belt have formed a tentative and uncertain alliance still haunted by a history of wars and prejudices. On the lost colony world of Laconia, a hidden enemy has a new vision for all of humanity and the power to enforce it.<br><br>New technologies clash with old as the history of human conflict returns to its ancient patterns of war and subjugation. But human nature is not the only enemy, and the forces being unleashed have their own price. A price that will change the shape of humanity -- and of the Rocinante -- unexpectedly and forever...</p>", "978-0-316-33283-5", "\\images\\product\\PersepolisRising.jpg", 300000.0, 290000.0, 260000.0, 280000.0, "Persepolis Rising	", new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9853) },
                    { new Guid("2d388a1d-9157-4609-af5a-12cfb253dcf7"), "Corey, James S. A.", new Guid("08366134-aa61-4b97-8f9b-1aa17d66b242"), new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9720), "<p>Abaddon's Gate is the third book in the New York Times bestselling Expanse series.<br><br>For generations, the solar system - Mars, the Moon, the Asteroid Belt - was humanity's great frontier. Until now. The alien artefact working through its program under the clouds of Venus has emerged to build a massive structure outside the orbit of Uranus: a gate that leads into a starless dark.<br><br>Jim Holden and the crew of the Rocinante are part of a vast flotilla of scientific and military ships going out to examine the artefact. But behind the scenes, a complex plot is unfolding, with the destruction of Holden at its core. As the emissaries of the human race try to find whether the gate is an opportunity or a threat, the greatest danger is the one they brought with them.</p>", "978-0-316-12907-7", "\\images\\product\\AbaddonsGate.jpg", 320000.0, 300000.0, 275000.0, 290000.0, "Abaddon's Gate", new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9720) },
                    { new Guid("46deb5b7-7ebc-4f20-8299-f2f5f637ed41"), "Corey, James S. A.", new Guid("08366134-aa61-4b97-8f9b-1aa17d66b242"), new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9857), "<p>Thirteen hundred gates have opened to solar systems around the galaxy. But as humanity builds its interstellar empire in the alien ruins, the mysteries and threats grow deeper.<br><br>In the dead systems where gates lead to stranger things than alien planets, Elvi Okoye begins a desperate search to discover the nature of a genocide that happened before the first human beings existed, and to find weapons to fight a war against forces at the edge of the imaginable. But the price of that knowledge may be higher than she can pay.<br><br>At the heart of the empire, Teresa Duarte prepares to take on the burden of her father's godlike ambition. The sociopathic scientist Paolo Cort&aacute;zar and the Mephistophelian prisoner James Holden are only two of the dangers in a palace thick with intrigue, but Teresa has a mind of her own and secrets even her father the emperor doesn't guess.<br><br>And throughout the wide human empire, the scattered crew of the Rocinante fights a brave rear-guard action against Duarte's authoritarian regime. Memory of the old order falls away, and a future under Laconia's eternal rule -- and with it, a battle that humanity can only lose - seems more and more certain. Because against the terrors that lie between worlds, courage and ambition will not be enough...</p>", "978-0-316-33286-6", "\\images\\product\\TiamatsWrath.jpg", 320000.0, 300000.0, 275000.0, 290000.0, "Tiamat's Wrath", new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9858) },
                    { new Guid("56ebfa27-972b-49a2-816a-2b4a2f40ad40"), "Corey, James S. A.", new Guid("08366134-aa61-4b97-8f9b-1aa17d66b242"), new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9861), "<p>The Laconian Empire has fallen, setting the thirteen hundred solar systems free from the rule of Winston Duarte. But the ancient enemy that killed the gate builders is awake, and the war against our universe has begun again.<br><br>In the dead system of Adro, Elvi Okoye leads a desperate scientific mission to understand what the gate builders were and what destroyed them, even if it means compromising herself and the half-alien children who bear the weight of her investigation. Through the wide-flung systems of humanity, Colonel Aliana Tanaka hunts for Duarte&rsquo;s missing daughter. . . and the shattered emperor himself. And on the Rocinante, James Holden and his crew struggle to build a future for humanity out of the shards and ruins of all that has come before.<br><br>As nearly unimaginable forces prepare to annihilate all human life, Holden and a group of unlikely allies discover a last, desperate chance to unite all of humanity, with the promise of a vast galactic civilization free from wars, factions, lies, and secrets if they win.<br><br>But the price of victory may be worse than the cost of defeat.</p>", "978-0-316-33291-0", "\\images\\product\\LeviathanFalls.jpg", 330000.0, 320000.0, 300000.0, 310000.0, "Leviathan Falls", new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9861) },
                    { new Guid("5a185130-6a85-4782-8c40-e108fbd46bef"), "Corey, James S. A.", new Guid("08366134-aa61-4b97-8f9b-1aa17d66b242"), new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9727), "<p>The fifth novel in Corey's New York Times bestselling Expanse series--now being produced for television by the SyFy Channel!<br><br>A thousand worlds have opened, and the greatest land rush in human history has begun. As wave after wave of colonists leave, the power structures of the old solar system begin to buckle.<br><br>Ships are disappearing without a trace. Private armies are being secretly formed. The sole remaining protomolecule sample is stolen. Terrorist attacks previously considered impossible bring the inner planets to their knees. The sins of the past are returning to exact a terrible price.<br><br>And as a new human order is struggling to be born in blood and fire, James Holden and the crew of the Rocinante must struggle to survive and get back to the only home they have left.</p>", "978-0-316-21758-3", "\\images\\product\\NemesisGames.jpg", 300000.0, 290000.0, 260000.0, 280000.0, "Nemesis Games", new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9727) },
                    { new Guid("72807004-91e6-4874-9cf0-409864a3bf36"), "Corey, James S. A.", new Guid("08366134-aa61-4b97-8f9b-1aa17d66b242"), new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9734), "<p>A revolution brewing for generations has begun in fire. It will end in blood.</p>\r\n<p>The Free Navy - a violent group of Belters in black-market military ships - has crippled the Earth and begun a campaign of piracy and violence among the outer planets. The colony ships heading for the thousand new worlds on the far side of the alien ring gates are easy prey, and no single navy remains strong enough to protect them.</p>\r\n<p>James Holden and his crew know the strengths and weaknesses of this new force better than anyone. Outnumbered and outgunned, the embattled remnants of the old political powers call on the&nbsp;<em>Rocinante&nbsp;</em>for a desperate mission to reach Medina Station at the heart of the gate network.</p>\r\n<p>But the new alliances are as flawed as the old, and the struggle for power has only just begun. As the chaos grows, an alien mystery deepens. Pirate fleets, mutiny and betrayal may be the least of the&nbsp;<em>Rocinante</em>'s problems. And in the uncanny spaces past the ring gates, the choices of a few damaged and desperate people may determine the fate of more than just humanity.</p>", "978-0-316-33474-7", "\\images\\product\\BabylonsAshes.jpg", 320000.0, 300000.0, 275000.0, 290000.0, "Babylon's Ashes", new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9735) },
                    { new Guid("79a89f86-f4c8-4a80-980e-34f69b05e108"), "Corey, James S. A.", new Guid("08366134-aa61-4b97-8f9b-1aa17d66b242"), new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9723), "<p>The gates have opened the way to thousands of habitable planets, and the land rush has begun. Settlers stream out from humanity's home planets in a vast, poorly controlled flood, landing on a new world. Among them, the Rocinante, haunted by the vast, posthuman network of the protomolecule as they investigate what destroyed the great intergalactic society that built the gates and the protomolecule.<br><br>But Holden and his crew must also contend with the growing tensions between the settlers and the company which owns the official claim to the planet. Both sides will stop at nothing to defend what's theirs, but soon a terrible disease strikes and only Holden - with help from the ghostly Detective Miller - can find the cure.</p>", "978-0-316-21762-0", "\\images\\product\\CibolaBurn.jpg", 270000.0, 260000.0, 240000.0, 250000.0, "Cibola Burn", new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9724) },
                    { new Guid("8c4b44e0-4657-42c6-a8d1-61933da57ee1"), "Corey, James S. A.", new Guid("08366134-aa61-4b97-8f9b-1aa17d66b242"), new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9698), "<p>Humanity has colonized the solar system&mdash;Mars, the Moon, the Asteroid Belt and beyond&mdash;but the stars are still out of our reach.<br><br>Jim Holden is XO of an ice miner making runs from the rings of Saturn to the mining stations of the Belt. When he and his crew stumble upon a derelict ship, the&nbsp;<em>Scopuli</em>, they find themselves in possession of a secret they never wanted. A secret that someone is willing to kill for&mdash;and kill on a scale unfathomable to Jim and his crew. War is brewing in the system unless he can find out who left the ship and why.<br><br>Detective Miller is looking for a girl. One girl in a system of billions, but her parents have money and money talks. When the trail leads him to the&nbsp;<em>Scopuli</em>&nbsp;and rebel sympathizer Holden, he realizes that this girl may be the key to everything.<br><br>Holden and Miller must thread the needle between the Earth government, the Outer Planet revolutionaries, and secretive corporations&mdash;and the odds are against them. But out in the Belt, the rules are different, and one small ship can change the fate of the universe.</p>", "978-0-316-12908-4", "\\images\\product\\LeviathanWakes.jpg", 300000.0, 290000.0, 260000.0, 280000.0, "Leviathan Wakes", new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9698) },
                    { new Guid("c89e1e4c-3b59-4f97-a551-1aff731d2eca"), "Corey, James S. A.", new Guid("08366134-aa61-4b97-8f9b-1aa17d66b242"), new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9716), "<p>We are not alone. On Ganymede, breadbasket of the outer planets, a Martian marine watches as her platoon is slaughtered by a monstrous supersoldier. On Earth, a high-level politician struggles to prevent interplanetary war from reigniting. And on Venus, an alien protomolecule has overrun the planet, wreaking massive, mysterious changes and threatening to spread out into the solar system.<br><br>In the vast wilderness of space, James Holden and the crew of the&nbsp;<em>Rocinante</em> have been keeping the peace for the Outer Planets Alliance. When they agree to help a scientist search war-torn Ganymede for a missing child, the future of humanity rests on whether a single ship can prevent an alien invasion that may have already begun.</p>", "978-0-316-12906-0", "\\images\\product\\CalibansWar.jpg", 310000.0, 295000.0, 265000.0, 285000.0, "Caliban's War", new DateTime(2025, 12, 11, 15, 41, 8, 807, DateTimeKind.Local).AddTicks(9717) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("9f5eeeac-95fc-47ef-99fc-b775431e978b"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("be776611-b2de-4e6c-8a3c-77eac2d8c0ce"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("67daf9c6-7b27-4c3f-b9fb-0fa4f63e200f"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("6fb4b932-5c54-4bf5-9e02-2007a0cbcddb"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("d3c3e1cb-7146-436b-973d-7a9e17aad693"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("29bab671-2efd-4068-9cdd-2adb8e7a4618"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("2d388a1d-9157-4609-af5a-12cfb253dcf7"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("46deb5b7-7ebc-4f20-8299-f2f5f637ed41"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("56ebfa27-972b-49a2-816a-2b4a2f40ad40"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("5a185130-6a85-4782-8c40-e108fbd46bef"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("72807004-91e6-4874-9cf0-409864a3bf36"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("79a89f86-f4c8-4a80-980e-34f69b05e108"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("8c4b44e0-4657-42c6-a8d1-61933da57ee1"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c89e1e4c-3b59-4f97-a551-1aff731d2eca"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("08366134-aa61-4b97-8f9b-1aa17d66b242"));

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Created", "DisplayOrder", "Name", "Updated" },
                values: new object[,]
                {
                    { new Guid("0ad20032-5475-4885-8ed8-e16082db1470"), new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(8749), 2, "Action", new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(8749) },
                    { new Guid("4568668b-0bab-4a0e-a09d-1782a0c8b3fd"), new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(8751), 3, "History", new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(8752) },
                    { new Guid("d48c3ec1-0c05-426e-abc6-0732a11bbbab"), new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(8688), 1, "Scifi", new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(8703) }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "City", "Created", "Name", "PhoneNumber", "PostalCode", "State", "StreetAddress", "Updated" },
                values: new object[,]
                {
                    { new Guid("2bada63b-74c8-4d07-98cb-b7403af35183"), "Tech City", new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(8918), "Tech Solution", "6669990000", "12121", "IL", "123 Tech St", new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(8918) },
                    { new Guid("6c7799c4-fe32-418d-9a01-b6f9e732e48a"), "Vid City", new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(8924), "Vivid Books", "7779990000", "66666", "IL", "999 Vid St", new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(8924) },
                    { new Guid("f1c43376-6547-4601-8a9e-a258c200a0b3"), "Lala land", new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(8926), "Readers Club", "1113335555", "99999", "NY", "999 Main St", new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(8926) }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "CategoryId", "Created", "Description", "ISBN", "ImageUrl", "ListPrice", "Price", "Price100", "Price50", "Title", "Updated" },
                values: new object[,]
                {
                    { new Guid("1a8d90be-d853-4aee-9412-a08c0526f03f"), "Corey, James S. A.", new Guid("d48c3ec1-0c05-426e-abc6-0732a11bbbab"), new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(9005), "<p>A revolution brewing for generations has begun in fire. It will end in blood.</p>\r\n<p>The Free Navy - a violent group of Belters in black-market military ships - has crippled the Earth and begun a campaign of piracy and violence among the outer planets. The colony ships heading for the thousand new worlds on the far side of the alien ring gates are easy prey, and no single navy remains strong enough to protect them.</p>\r\n<p>James Holden and his crew know the strengths and weaknesses of this new force better than anyone. Outnumbered and outgunned, the embattled remnants of the old political powers call on the&nbsp;<em>Rocinante&nbsp;</em>for a desperate mission to reach Medina Station at the heart of the gate network.</p>\r\n<p>But the new alliances are as flawed as the old, and the struggle for power has only just begun. As the chaos grows, an alien mystery deepens. Pirate fleets, mutiny and betrayal may be the least of the&nbsp;<em>Rocinante</em>'s problems. And in the uncanny spaces past the ring gates, the choices of a few damaged and desperate people may determine the fate of more than just humanity.</p>", "978-0-316-33474-7", "\\images\\product\\BabylonsAshes.jpg", 320000.0, 300000.0, 275000.0, 290000.0, "Babylon's Ashes", new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(9006) },
                    { new Guid("5eec2c35-8c32-49e6-af2b-5c2d482041d1"), "Corey, James S. A.", new Guid("d48c3ec1-0c05-426e-abc6-0732a11bbbab"), new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(9011), "<p>Thirteen hundred gates have opened to solar systems around the galaxy. But as humanity builds its interstellar empire in the alien ruins, the mysteries and threats grow deeper.<br><br>In the dead systems where gates lead to stranger things than alien planets, Elvi Okoye begins a desperate search to discover the nature of a genocide that happened before the first human beings existed, and to find weapons to fight a war against forces at the edge of the imaginable. But the price of that knowledge may be higher than she can pay.<br><br>At the heart of the empire, Teresa Duarte prepares to take on the burden of her father's godlike ambition. The sociopathic scientist Paolo Cort&aacute;zar and the Mephistophelian prisoner James Holden are only two of the dangers in a palace thick with intrigue, but Teresa has a mind of her own and secrets even her father the emperor doesn't guess.<br><br>And throughout the wide human empire, the scattered crew of the Rocinante fights a brave rear-guard action against Duarte's authoritarian regime. Memory of the old order falls away, and a future under Laconia's eternal rule -- and with it, a battle that humanity can only lose - seems more and more certain. Because against the terrors that lie between worlds, courage and ambition will not be enough...</p>", "978-0-316-33286-6", "\\images\\product\\TiamatsWrath.jpg", 320000.0, 300000.0, 275000.0, 290000.0, "Tiamat's Wrath", new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(9011) },
                    { new Guid("6b87d71b-586f-4fee-a049-dba00f2f8bc8"), "Corey, James S. A.", new Guid("d48c3ec1-0c05-426e-abc6-0732a11bbbab"), new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(9014), "<p>The Laconian Empire has fallen, setting the thirteen hundred solar systems free from the rule of Winston Duarte. But the ancient enemy that killed the gate builders is awake, and the war against our universe has begun again.<br><br>In the dead system of Adro, Elvi Okoye leads a desperate scientific mission to understand what the gate builders were and what destroyed them, even if it means compromising herself and the half-alien children who bear the weight of her investigation. Through the wide-flung systems of humanity, Colonel Aliana Tanaka hunts for Duarte&rsquo;s missing daughter. . . and the shattered emperor himself. And on the Rocinante, James Holden and his crew struggle to build a future for humanity out of the shards and ruins of all that has come before.<br><br>As nearly unimaginable forces prepare to annihilate all human life, Holden and a group of unlikely allies discover a last, desperate chance to unite all of humanity, with the promise of a vast galactic civilization free from wars, factions, lies, and secrets if they win.<br><br>But the price of victory may be worse than the cost of defeat.</p>", "978-0-316-33291-0", "\\images\\product\\LeviathanFalls.jpg", 330000.0, 320000.0, 300000.0, 310000.0, "Leviathan Falls", new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(9014) },
                    { new Guid("774a3b61-bd46-480f-baf6-1afb2871736a"), "Corey, James S. A.", new Guid("d48c3ec1-0c05-426e-abc6-0732a11bbbab"), new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(9002), "<p>The fifth novel in Corey's New York Times bestselling Expanse series--now being produced for television by the SyFy Channel!<br><br>A thousand worlds have opened, and the greatest land rush in human history has begun. As wave after wave of colonists leave, the power structures of the old solar system begin to buckle.<br><br>Ships are disappearing without a trace. Private armies are being secretly formed. The sole remaining protomolecule sample is stolen. Terrorist attacks previously considered impossible bring the inner planets to their knees. The sins of the past are returning to exact a terrible price.<br><br>And as a new human order is struggling to be born in blood and fire, James Holden and the crew of the Rocinante must struggle to survive and get back to the only home they have left.</p>", "978-0-316-21758-3", "\\images\\product\\NemesisGames.jpg", 300000.0, 290000.0, 260000.0, 280000.0, "Nemesis Games", new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(9003) },
                    { new Guid("778209d9-2e72-46ab-a63d-aa0da95a2b84"), "Corey, James S. A.", new Guid("d48c3ec1-0c05-426e-abc6-0732a11bbbab"), new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(8972), "<p>Abaddon's Gate is the third book in the New York Times bestselling Expanse series.<br><br>For generations, the solar system - Mars, the Moon, the Asteroid Belt - was humanity's great frontier. Until now. The alien artefact working through its program under the clouds of Venus has emerged to build a massive structure outside the orbit of Uranus: a gate that leads into a starless dark.<br><br>Jim Holden and the crew of the Rocinante are part of a vast flotilla of scientific and military ships going out to examine the artefact. But behind the scenes, a complex plot is unfolding, with the destruction of Holden at its core. As the emissaries of the human race try to find whether the gate is an opportunity or a threat, the greatest danger is the one they brought with them.</p>", "978-0-316-12907-7", "\\images\\product\\AbaddonsGate.jpg", 320000.0, 300000.0, 275000.0, 290000.0, "Abaddon's Gate", new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(8973) },
                    { new Guid("84d896be-2f5a-454b-a93e-9719df3cf516"), "Corey, James S. A.", new Guid("d48c3ec1-0c05-426e-abc6-0732a11bbbab"), new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(8950), "<p>Humanity has colonized the solar system&mdash;Mars, the Moon, the Asteroid Belt and beyond&mdash;but the stars are still out of our reach.<br><br>Jim Holden is XO of an ice miner making runs from the rings of Saturn to the mining stations of the Belt. When he and his crew stumble upon a derelict ship, the&nbsp;<em>Scopuli</em>, they find themselves in possession of a secret they never wanted. A secret that someone is willing to kill for&mdash;and kill on a scale unfathomable to Jim and his crew. War is brewing in the system unless he can find out who left the ship and why.<br><br>Detective Miller is looking for a girl. One girl in a system of billions, but her parents have money and money talks. When the trail leads him to the&nbsp;<em>Scopuli</em>&nbsp;and rebel sympathizer Holden, he realizes that this girl may be the key to everything.<br><br>Holden and Miller must thread the needle between the Earth government, the Outer Planet revolutionaries, and secretive corporations&mdash;and the odds are against them. But out in the Belt, the rules are different, and one small ship can change the fate of the universe.</p>", "978-0-316-12908-4", "\\images\\product\\LeviathanWakes.jpg", 300000.0, 290000.0, 260000.0, 280000.0, "Leviathan Wakes", new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(8951) },
                    { new Guid("d355381b-b235-492a-a812-d6f2bb4d51eb"), "Corey, James S. A.", new Guid("d48c3ec1-0c05-426e-abc6-0732a11bbbab"), new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(9008), "<p>In the thousand-sun network of humanity's expansion, new colony worlds are struggling to find their way. Every new planet lives on a knife edge between collapse and wonder, and the crew of the aging gunship Rocinante have their hands more than full keeping the fragile peace.<br><br>In the vast space between Earth and Jupiter, the inner planets and belt have formed a tentative and uncertain alliance still haunted by a history of wars and prejudices. On the lost colony world of Laconia, a hidden enemy has a new vision for all of humanity and the power to enforce it.<br><br>New technologies clash with old as the history of human conflict returns to its ancient patterns of war and subjugation. But human nature is not the only enemy, and the forces being unleashed have their own price. A price that will change the shape of humanity -- and of the Rocinante -- unexpectedly and forever...</p>", "978-0-316-33283-5", "\\images\\product\\PersepolisRising.jpg", 300000.0, 290000.0, 260000.0, 280000.0, "Persepolis Rising	", new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(9009) },
                    { new Guid("eab18715-670c-439b-82dc-5fb36eef2a54"), "Corey, James S. A.", new Guid("d48c3ec1-0c05-426e-abc6-0732a11bbbab"), new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(8969), "<p>We are not alone. On Ganymede, breadbasket of the outer planets, a Martian marine watches as her platoon is slaughtered by a monstrous supersoldier. On Earth, a high-level politician struggles to prevent interplanetary war from reigniting. And on Venus, an alien protomolecule has overrun the planet, wreaking massive, mysterious changes and threatening to spread out into the solar system.<br><br>In the vast wilderness of space, James Holden and the crew of the&nbsp;<em>Rocinante</em> have been keeping the peace for the Outer Planets Alliance. When they agree to help a scientist search war-torn Ganymede for a missing child, the future of humanity rests on whether a single ship can prevent an alien invasion that may have already begun.</p>", "978-0-316-12906-0", "\\images\\product\\CalibansWar.jpg", 310000.0, 295000.0, 265000.0, 285000.0, "Caliban's War", new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(8969) },
                    { new Guid("ed47737e-c460-4a0c-bd3e-a5cacc9ea860"), "Corey, James S. A.", new Guid("d48c3ec1-0c05-426e-abc6-0732a11bbbab"), new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(8999), "<p>The gates have opened the way to thousands of habitable planets, and the land rush has begun. Settlers stream out from humanity's home planets in a vast, poorly controlled flood, landing on a new world. Among them, the Rocinante, haunted by the vast, posthuman network of the protomolecule as they investigate what destroyed the great intergalactic society that built the gates and the protomolecule.<br><br>But Holden and his crew must also contend with the growing tensions between the settlers and the company which owns the official claim to the planet. Both sides will stop at nothing to defend what's theirs, but soon a terrible disease strikes and only Holden - with help from the ghostly Detective Miller - can find the cure.</p>", "978-0-316-21762-0", "\\images\\product\\CibolaBurn.jpg", 270000.0, 260000.0, 240000.0, 250000.0, "Cibola Burn", new DateTime(2025, 12, 11, 15, 34, 53, 560, DateTimeKind.Local).AddTicks(9000) }
                });
        }
    }
}
