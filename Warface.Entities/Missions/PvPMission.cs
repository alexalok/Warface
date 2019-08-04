using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Warface.Entities.Missions
{
    public class PvPMission : Mission
    {
        public PvPMission(string key, string type) : base(key, type, MissionMode.Pvp, false)
        {
        }

        public new static PvPMission ParseNode(HtmlNode missionNode)
        {
            /*<mission mission_key='cd54d2eb-f00e-4ccc-bbd4-d4c0f2cc935e' no_teams='0' name='@pvp_mission_display_name_stm_wharf' 
              * setting='pvp/stm_wharf' mode='stm' mode_name='@pvp_stm_game_mode_desc' mode_icon='tdm_icon' 
               * description='@pvp_stm_mission_desc' image='mapImgStmWharf' difficulty='normal' type='' time_of_day='09.30' revision='668'>*/

            string missionKey = missionNode.Attributes["mission_key"].Value;
            return PvpMissions.PvpMissionsList.First(m => m.Key == missionKey);
        }
    }

    public static class PvpMissions
    {
        public static readonly List<PvPMission> PvpMissionsList;

        static PvpMissions() => PvpMissionsList = new List<PvPMission>
        {
            new PvPMission("45188feb-0017-46ec-bb57-189dbbd6be2d", "ctf_breach"),
            new PvPMission("73904867-1107-4db2-a7eb-4593d580f835", "ctf_construction"),
            new PvPMission("f32bc4ca-6420-4034-82d6-c8041e3bd3fa", "ctf_convoy"),
            new PvPMission("d5da06ce-a425-4013-abd8-6f9bfbe9f763", "ctf_deposit"),
            new PvPMission("7684fdcf-f6aa-431e-9616-608eccd3a847", "ctf_longway"),
            new PvPMission("f32bc8e2-06df-413a-935a-b039ee4e4467", "ctf_quarry"),
            new PvPMission("6f7644a3-1a37-4cbb-ab01-128a1015b64f", "ctf_test"),
            new PvPMission("dfeebeda-783e-4fe2-98fa-a76bf35b31b9", "ctf_vault_fwc"),
            new PvPMission("60c04653-a419-433f-a43b-63278d8bd339", "ctf_vault"),
            new PvPMission("e1129d41-42fd-4451-b729-4b7ff5f4e3e7", "dmn_armageddon"),
            new PvPMission("0726fb42-1d41-7a43-97d1-4e0571124cd8", "dmn_downtown"),
            new PvPMission("70e29d78-bb85-4014-8989-ffeb9074d2bc", "dmn_sirius"),
            new PvPMission("d0af9f60-6402-11c4-8d37-8c89a553425b", "dmn_subzero"),
            new PvPMission("d1f15bf2-1db4-42f4-b335-097a57b83ed4", "dst_afghan"),
            new PvPMission("ea0cbac2-5891-4d37-bb42-09e790730b34", "dst_lighthouse"),
            new PvPMission("e6e96230-b938-11e5-b925-8c89a554425b", "ffa_bunker"),
            new PvPMission("cccea92f-612d-2850-1111-3e02a3a54d6c", "ffa_downtown"),
            new PvPMission("fb53bbbb-f555-45aa-9144-8442446b1c4f", "ffa_forest_dawn"),
            new PvPMission("227123ef-b394-4321-b697-abed0fff9e38", "ffa_forest"),
            new PvPMission("4eac367d-a583-4640-a56b-3e8d3d6d2cf1", "ffa_japan"),
            new PvPMission("dda0b8ac-de2e-9474-84fc-5b4ec51bf7ef", "ffa_motel"),
            new PvPMission("52fa65a0-d182-4abb-97c2-54857c6b3bbc", "ffa_overpass"),
            new PvPMission("c61ea94f-505d-4860-9199-3e02a1a54d9c", "ffa_train"),
            new PvPMission("45e182f3-d45c-4e5d-8054-fc4bbbfc2782", "ffa_widestreet"),
            new PvPMission("f22283aa-47a8-40d1-bd06-2655156274fe", "ffa_widestreet_up"),
            new PvPMission("2124463b-0612-4882-a5de-b222b21bf0e6", "hnt_africa"),
            new PvPMission("959ddc2d-9bb4-4eab-9be8-8e04e76d94aa", "hnt_night_motel"),
            new PvPMission("d34538bc-f603-4f39-afe5-db27000564cd", "hnt_winter"),
            new PvPMission("bee96351-e973-4222-a145-5a68a6773bba", "hnt_winter_xmas"),
            new PvPMission("3889f29a-fe06-425b-9ff8-f94ffbd5307c", "lms_mojave"),
            new PvPMission("e246660c-52cb-4a56-8d25-c0f040d03793", "lms_pripyat"),
            new PvPMission("123bec5b-a1c2-4bab-b105-3b5b89bab014", "ptb_afghan"),
            new PvPMission("8b7189c1-6c6e-413f-96b8-bb4b0abb2069", "ptb_afghan_up"),
            new PvPMission("3c3c5677-02df-4b87-91b1-8a61bcb16bcf", "ptb_bridges"),
            new PvPMission("2412db5e-c19a-4fe8-ba99-cd28cdcce388", "ptb_bridges_up"),
            new PvPMission("cdb2fe18-c99e-56b0-9e7f-ffc1d7733f53", "ptb_D17"),
            new PvPMission("543df61d-f277-4953-9e0f-3eb8c998fa3b", "ptb_destination"),
            new PvPMission("8a0f1f90-c1b5-4235-baef-81318bb6e519", "ptb_district"),
            new PvPMission("77e4f737-431c-4359-b27f-4800c5a5b780", "ptb_factory"),
            new PvPMission("e00c96b8-8185-4cbf-9bd5-e70520021575", "ptb_mine"),
            new PvPMission("a2e98ff0-b602-44e5-b517-02518a7c19eb", "ptb_overpass"),
            new PvPMission("afcf6636-b5a8-4eec-ac24-fa0915695a15", "ptb_palace"),
            new PvPMission("b1594650-15ad-43c0-850f-863f5ab85ef5", "ptb_pyramid"),
            new PvPMission("3f57cdb0-010a-11e5-a726-8c99a553325b", "ptb_trailerpark"),
            new PvPMission("517d8a3f-91f8-4566-81a2-b48a70e44014", "stm_blackgold"),
            new PvPMission("ee01cd3a-d214-48fa-843b-788427a6161c", "stm_blackmamba"),
            new PvPMission("2a8cbd01-52ae-4783-bdd6-d2f858b4fd74", "stm_highhill"),
            new PvPMission("18a2580c-5988-4890-96a9-b7b40c5487c4", "stm_invasion"),
            new PvPMission("cd54d2eb-f00e-4ccc-bbd4-d4c0f2cc935e", "stm_wharf"),
            new PvPMission("e4f56e59-97fb-4451-a137-7b44026d96b0", "tdm_airbase"),
            new PvPMission("24531004-8e3f-48e3-a760-59f914c02c0d", "tdm_aul_pts"),
            new PvPMission("bbb33b77-98db-463c-9a1a-d27655d90690", "tdm_aul"),
            new PvPMission("f405283f-42f9-4d5e-b02a-618cacd10cf4", "tdm_codenameD18"),
            new PvPMission("15144c77-4aaa-4d33-a88b-06eee80fd7f7", "tdm_crossriver"),
            new PvPMission("51a9ebbc-8519-4f5e-b83e-0e9911c6995c", "tdm_dock"),
            new PvPMission("0726fb17-1d35-4a43-97d1-4e0563794cd8", "tdm_downtown"),
            new PvPMission("400e7fee-3fda-11e4-baf7-d43d7e9be5ba", "tdm_farm_hw"),
            new PvPMission("90c7e3c1-6432-4745-8abc-40cbccaa4d25", "tdm_farm_sunset"),
            new PvPMission("30afabfd-032f-4ff3-a22e-8b889349aaa5", "tdm_farm"),
            new PvPMission("3347d361-ef36-4c57-8bca-4f8051fc97e6", "tdm_grand_bazaar"),
            new PvPMission("b4ac3d2f-1e97-4132-800e-7e1dbcd3512f", "tdm_ghost_town"),
            new PvPMission("e5981b6a-325d-42eb-a3fe-e6eed0bc4bf2", "tdm_hangar_up"),
            new PvPMission("953740a3-022e-4243-9044-eb1c07a2f680", "tdm_hangar_xmas"),
            new PvPMission("d42df33c-bbaa-49d9-bed2-8db61ef5b233", "tdm_hangar"),
            new PvPMission("bf506d56-21cd-49e3-b57a-4b1e6f66ba8d", "tdm_hangar_og16"),
            new PvPMission("71e863e1-e28c-489d-b145-866987fbe031", "tdm_motel"),
            new PvPMission("0575ee9b-db2a-4a8d-a376-c2ec4b021587", "tdm_oildepot_e16"),
            new PvPMission("1d71c946-7ffc-4045-8035-14eea4072e02", "tdm_oildepotv3"),
            new PvPMission("24578605-b482-4dcf-af0a-7a6968732602", "tdm_oildepot_xmas"),
            new PvPMission("805b24e5-f5b6-447c-b7a5-c82c6b0134cb", "tdm_residential"),
            new PvPMission("8702f1ee-bac1-4bea-a92b-d5b76c5cd5af", "tdm_shuttle"),
            new PvPMission("c090ccd6-dfd4-46aa-a5a6-929d974451e1", "tdm_sirius"),
            new PvPMission("d2cc9728-2a3f-4387-a0ea-ea1e0f967495", "tdm_streetwars"),
            new PvPMission("a8ee23a0-da5a-4a25-823f-23dcf98198c2", "tbs_deepwater"),
            new PvPMission("791f8fd3-41de-4a94-a5d0-945b461f691e", "tbs_hawkrock"),
            new PvPMission("da20dcc4-9672-4077-b877-1b04c6772b75", "tbs_waterfalling"),
            new PvPMission("8fa3688a-75c9-44d8-ff92-90ac38c56b4c", "tbs_waterfalling_xmas"),
        };
    }
}