using Daily_Metting.Models;

namespace Daily_Metting.Data
{
    public class DBInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            DailyMeetingDbContext context = applicationBuilder.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<DailyMeetingDbContext>();

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(Categories.Select(c => c.Value));
            }

            if (!context.Points.Any())
            {
                context.AddRange
                (
                    //Safety Points
                    new Point { Point_Name = "Fatalities",WH_Acces=true,CS_PP_Acces=true,Procurement_Acces=true, HasMultipleValues=false, Category = Categories["Safety"] },
                    new Point { Point_Name = "Major Accidents", WH_Acces = true, CS_PP_Acces = true, Procurement_Acces = true, HasMultipleValues = false, Category = Categories["Safety"] },
                    new Point { Point_Name = "Minor Accidents", WH_Acces = true, CS_PP_Acces = true, Procurement_Acces = true, HasMultipleValues = false, Category = Categories["Safety"] },
                    new Point { Point_Name = "Near Misses", WH_Acces = true, CS_PP_Acces = true, Procurement_Acces = true, HasMultipleValues = false, Category = Categories["Safety"] },
                    new Point { Point_Name = "Unsafe Acts", WH_Acces = true, CS_PP_Acces = true, Procurement_Acces = true, HasMultipleValues = false, Category = Categories["Safety"] },

                    //People                   
                    new Point { Point_Name = "Absenteism", WH_Acces = true, CS_PP_Acces = true, Procurement_Acces = true, HasMultipleValues = false, Category = Categories["People"] },
                    new Point { Point_Name = "Vacancies", WH_Acces = true, CS_PP_Acces = true, Procurement_Acces = true, HasMultipleValues = false, Category = Categories["People"] },
                    new Point { Point_Name = "Others", WH_Acces = true, CS_PP_Acces = true, Procurement_Acces = true, HasMultipleValues = false, Category = Categories["People"] },

                    //Quality
                    new Point { Point_Name = "Customer Claims", WH_Acces = false, CS_PP_Acces = true, Procurement_Acces = false, HasMultipleValues = false, Category = Categories["Quality"] },
                    new Point { Point_Name = "Blocked FG (€)", WH_Acces = false, CS_PP_Acces = true, Procurement_Acces = false, HasMultipleValues = true, Category = Categories["Quality"] },
                    new Point { Point_Name = "Supplier Claims",WH_Acces=false,CS_PP_Acces=false,Procurement_Acces=true, HasMultipleValues = false, Category = Categories["Quality"] },
                    new Point { Point_Name = "Blocked RM (€)", WH_Acces = false, CS_PP_Acces = false, Procurement_Acces = true, HasMultipleValues = true, Category = Categories["Quality"] },
                    new Point { Point_Name = "RM Stock", WH_Acces = false, CS_PP_Acces = false, Procurement_Acces = true, HasMultipleValues = false, Category = Categories["Quality"] },
                    new Point { Point_Name = "RM Finish good", WH_Acces = false, CS_PP_Acces = false, Procurement_Acces = true, HasMultipleValues = false, Category = Categories["Quality"] },
                    new Point { Point_Name = "RM Global Stock", WH_Acces = false, CS_PP_Acces = false, Procurement_Acces = true, HasMultipleValues = false, Category = Categories["Quality"] },
                    new Point { Point_Name = "Expiration Status", WH_Acces = false, CS_PP_Acces = false, Procurement_Acces = true, HasMultipleValues = true, Category = Categories["Quality"] },

                    //Delivery
                    new Point { Point_Name = "Customer Escallation", WH_Acces = false, CS_PP_Acces = true, Procurement_Acces = false, HasMultipleValues = false, Category = Categories["Delivery"] },
                    new Point { Point_Name = "PP related Line Stoppage", WH_Acces = false, CS_PP_Acces = true, Procurement_Acces = false, HasMultipleValues = false, Category = Categories["Delivery"] },
                    new Point { Point_Name = "Current Wk Criticals List", WH_Acces = false, CS_PP_Acces = false, Procurement_Acces = true, HasMultipleValues = true, Category = Categories["Delivery"] },
                    new Point { Point_Name = "Wk+1 Criticals List", WH_Acces = false, CS_PP_Acces = false, Procurement_Acces = true , HasMultipleValues = true, Category = Categories["Delivery"] },
                    new Point { Point_Name = "Wk+2 Criticals List", WH_Acces = false, CS_PP_Acces = false, Procurement_Acces = true, HasMultipleValues = true, Category = Categories["Delivery"] },
                    new Point { Point_Name = "Wk+3 Criticals List", WH_Acces = false, CS_PP_Acces = false, Procurement_Acces = true, HasMultipleValues = true, Category = Categories["Delivery"] },
                    new Point { Point_Name = "RM related Line Stoppage", WH_Acces = false, CS_PP_Acces = false, Procurement_Acces = true, HasMultipleValues = false, Category = Categories["Delivery"] },
                    new Point { Point_Name = "Line Feeding Line Stoppage", WH_Acces = true, CS_PP_Acces = false, Procurement_Acces = false, HasMultipleValues = false, Category = Categories["Delivery"] },
                    new Point { Point_Name = "COGI", WH_Acces = true, CS_PP_Acces = false, Procurement_Acces = false, HasMultipleValues = false, Category = Categories["Delivery"] },
                    new Point { Point_Name = "Cycle Count RM", WH_Acces = true, CS_PP_Acces = false, Procurement_Acces = false, HasMultipleValues = false, Category = Categories["Delivery"] },
                    new Point { Point_Name = "Cycle Count WIP", WH_Acces = true, CS_PP_Acces = false, Procurement_Acces = false, HasMultipleValues = false, Category = Categories["Delivery"] },
                    new Point { Point_Name = "Cycle Count FG", WH_Acces = true, CS_PP_Acces = false, Procurement_Acces = false, HasMultipleValues = false, Category = Categories["Delivery"] },
                    new Point { Point_Name = "Outbound truck Status", WH_Acces = true, CS_PP_Acces = false, Procurement_Acces = false, HasMultipleValues = false, Category = Categories["Delivery"] },
                    new Point { Point_Name = "Inbound truck Status", WH_Acces = true, CS_PP_Acces = false, Procurement_Acces = false, HasMultipleValues = false, Category = Categories["Delivery"] },
                    new Point { Point_Name = "GR Onhold Area", WH_Acces = true, CS_PP_Acces = false, Procurement_Acces = false, HasMultipleValues = true, Category = Categories["Delivery"] },
                    new Point { Point_Name = "Transport Claims", WH_Acces = true, CS_PP_Acces = false, Procurement_Acces = false, HasMultipleValues = false, Category = Categories["Delivery"] },

                    //Cost
                    new Point { Point_Name = "Extra RM Cost", WH_Acces = false, CS_PP_Acces = false, Procurement_Acces = true, HasMultipleValues = false, Category = Categories["Cost"] },
                    new Point { Point_Name = "FG Special Transport", WH_Acces = false, CS_PP_Acces = true, Procurement_Acces = true, HasMultipleValues = false, Category = Categories["Cost"] },
                    new Point { Point_Name = "RM special Transport" ,WH_Acces = false, CS_PP_Acces = false, Procurement_Acces = true, HasMultipleValues = false, Category = Categories["Cost"] }
                    );


            }




            context.SaveChanges();
        }

        private static Dictionary<string, Category>? categories;

        public static Dictionary<string, Category> Categories
        {
            get
            {
                if (categories == null)
                {
                    var genresList = new Category[]
                    {
                        new Category { Category_Name = "Safety" },
                        new Category { Category_Name = "People" },
                        new Category { Category_Name = "Quality" },
                        new Category { Category_Name = "Delivery" },
                        new Category { Category_Name = "Cost" }
                    };

                    categories = new Dictionary<string, Category>();

                    foreach (Category genre in genresList)
                    {
                        categories.Add(genre.Category_Name, genre);
                    }
                }

                return categories;
            }
        }
    }
}

