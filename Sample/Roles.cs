using Greatbone;

namespace Samp
{
    public class MyWork : Work
    {
        public MyWork(WorkConfig cfg) : base(cfg)
        {
            CreateVar<MyVarWork, int>((obj) => ((User) obj).id);
        }
    }

    [UserAccess(hubly: 1)]
    [Ui("首页")]
    public class HubWork : Work
    {
        public HubWork(WorkConfig cfg) : base(cfg)
        {
            Create<HubOrderWork>("order");

            Create<HubItemWork>("item");

            Create<HubChatWork>("chat");

            Create<HubUserWork>("user");

            Create<HubOrgWork>("org");

            Create<HubRepayWork>("repay");
        }

        public void @default(WebContext wc)
        {
            bool inner = wc.Query[nameof(inner)];
            if (inner)
            {
                using (var dc = NewDbContext())
                {
                    wc.GivePage(200, h =>
                    {
                        h.TOOLBAR();
                        h.SECTION_("uk-card uk-card-default");
                        h.HEADER_("uk-card-header").H4("网点")._HEADER();
                        h.MAIN_("uk-card-body")._MAIN();
                        h._SECTION();

                        h.SECTION_("uk-card uk-card-default");
                        h.HEADER_("uk-card-header").H4("订单")._HEADER();
                        h.MAIN_("uk-card-body")._MAIN();
                        h._SECTION();
                    });
                }
            }
            else
            {
                wc.GiveFrame(200, false, 900, "调度服务");
            }
        }
    }

    public class ShopWork : Work
    {
        public ShopWork(WorkConfig cfg) : base(cfg)
        {
            CreateVar<ShopVarWork, short>(prin => ((User) prin).shopat);
        }
    }


    public class TeamWork : Work
    {
        public TeamWork(WorkConfig cfg) : base(cfg)
        {
            CreateVar<TeamVarWork, short>(prin => ((User) prin).teamat);
        }
    }
}