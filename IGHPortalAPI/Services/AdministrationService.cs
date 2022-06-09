using AutoMapper;
using IGHportalAPI.DataContext;
using IGHportalAPI.Models;
using IGHportalAPI.ViewModels.AccountViewModels;
using IGHportalAPI.ViewModels.AdministrationViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IGHportalAPI.Models.Enums;

namespace IGHportalAPI.Services
{
    public class AdministrationService : IAdministrationService
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private readonly DataContext_ db;
        private readonly IMapper mapper;

        public AdministrationService(RoleManager<IdentityRole> roleManager,
                                        UserManager<User> userManager,
                                        DataContext_ db,
                                        IMapper mapper)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.db = db;
            this.mapper = mapper;
        }


        public async Task<IdentityResult> CreateRoleAsync(RoleViewModel roleModel)
        {
            IdentityRole identityRole = new IdentityRole()
            {
                Name = roleModel.Name
            };

            IdentityResult result = await roleManager.CreateAsync(identityRole);


            if (result.Succeeded)
            {
                return result;
            }
            else
            {
                return IdentityResult.Failed();
            }
        }


        public IEnumerable<RoleViewModel> GetAllRoles()
        {
            var AllRoles = roleManager.Roles.Select(a => new RoleViewModel() { Id = a.Id, Name = a.Name });

            return AllRoles.OrderByDescending(a => a.Id);
        }
        //---------------------------------------------------------
        public async Task<IdentityResult> DeleteRole(string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);

            //IdentityRole role1 = new IdentityRole()
            //{
            //    Id = Id
            //};

            var result = await roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return result;
            }
            else
            {
                return IdentityResult.Failed();
            }

        }

        //----------------------------------------
        //----------------------------------------
        public async Task<List<AllRolesViewModel>> AllRolesInfo()
        {
            var AllRoles = roleManager.Roles.ToList();

            var ListRolesDTO = mapper.Map<List<AllRolesViewModel>>(AllRoles);

            foreach (var role in ListRolesDTO)
            {
                foreach (var user in userManager.Users)
                {
                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        role.Users.Add(user.UserName);
                    }
                }
            }
            return ListRolesDTO;
        }
        //----------------------------------------
        //----------------------------------------
        public async Task<RoleViewModel> GetRoleByID(string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);
            
            var Object = mapper.Map<RoleViewModel>(role);

            return Object;
        }

        /*---------------------------------*/
        public async Task<IdentityResult> EditRolePostAsync(RoleViewModel model)
        {
            IdentityRole role = await roleManager.FindByIdAsync(model.Id);




            if (role != null)
            {
                role.Name = model.Name;
                var Result = await roleManager.UpdateAsync(role);

                return Result;
            }
            else
            {
                return IdentityResult.Failed();
            }
        }


        private async Task EditUserHotels(IList<string> SelectedRoles, User user)
        {
            var AllHotelsFromDB = db.Hotels.ToList();


            foreach (var hotelFromDb in AllHotelsFromDB)
            {
                var HotelsUsersObj = db.HotelUsers.AsNoTracking().Where(a => a.HotelsId == hotelFromDb.Id && a.UsersId == user.Id).FirstOrDefault();

                if (  (HotelsUsersObj != null) && SelectedRoles.Contains(hotelFromDb.Name))
                {
                    continue;
                }
                else if ((HotelsUsersObj != null) && (!SelectedRoles.Contains(hotelFromDb.Name)))
                {

                    db.Entry(HotelsUsersObj).State = EntityState.Deleted;
                    await db.SaveChangesAsync();
                }
                else if ((HotelsUsersObj == null) && (SelectedRoles.Contains(hotelFromDb.Name)))
                {
                    db.HotelUsers.Add(new IGHportalAPI.Models.HotelUser() { HotelsId = hotelFromDb.Id, UsersId = user.Id });
                     await db.SaveChangesAsync();
                }
            }




        }

        //-----------------------------------------
        private async Task<List<IdentityResult>> EditUserRoles(IList<string> SelectedRoles, User userObj)
        {

            var AllRolesFromDB = roleManager.Roles.ToList();

            var RolesToBAdded = new List<string>();
            var RolesToBdeleted = new List<string>();


            List<IdentityResult> ResultList = new List<IdentityResult>();

            foreach (var roleFromDb in AllRolesFromDB)
            {
                if (await userManager.IsInRoleAsync(userObj, roleFromDb.Name) && SelectedRoles.Contains(roleFromDb.Name))
                {
                    continue;
                }
                else if (await userManager.IsInRoleAsync(userObj, roleFromDb.Name) && (!SelectedRoles.Contains(roleFromDb.Name)))
                {
                    RolesToBdeleted.Add(roleFromDb.Name);
                }
                else if (!await userManager.IsInRoleAsync(userObj, roleFromDb.Name) && (SelectedRoles.Contains(roleFromDb.Name)))
                {
                    RolesToBAdded.Add(roleFromDb.Name);
                }
            }
            var Result = new IdentityResult();
            var Result2 = new IdentityResult();
            if (RolesToBAdded.Count > 0)
            {
                Result = await userManager.AddToRolesAsync(userObj, RolesToBAdded);
            }
            if (RolesToBdeleted.Count > 0)
            {
                Result2 = await userManager.RemoveFromRolesAsync(userObj, RolesToBdeleted);
            }


            ResultList.Add(Result);
            ResultList.Add(Result2);
            //var Result = await EditUserRoles(userModel.Roles, user);

            return ResultList;

        }

        //----------------------------------
        public  async Task<IEnumerable<UserViewModel>> GetListUsers()
        {
            var ListUsers = userManager.Users.Where(a => a.IsActive==true);

            var UserDTOList = mapper.Map<IEnumerable<UserViewModel>>(ListUsers).OrderByDescending(a => a.Id);

            foreach (var user in UserDTOList)
            {
                var UserObj = mapper.Map<User>(user);

                var HotelNamesOfUser =  db.HotelUsers.Where(a => a.UsersId == user.Id).Where(a => a.Hotel.UpdatedStatus < (short)UpdStatus.Deleted).Select(a => a.Hotel.Name).ToList();

                if(HotelNamesOfUser.Count > 0)
                {
                    user.Hotels = HotelNamesOfUser;
                }

                user.Roles = await userManager.GetRolesAsync(UserObj);
            }


                return UserDTOList;
        }

        //------------------------------------
        public async Task<EditUserViewModel> EditUserGet(string Id)
        {


            var user = await userManager.FindByIdAsync(Id);

            if (user != null)
            {

                //    var userClaims = await userManager.GetClaimsAsync(user);
                var userRoles = await userManager.GetRolesAsync(user);

                //userRoles.Add("RoleA");
                //List<UserRoleDTO> list = new List<UserRoleDTO>();
                //foreach (var item in userRoles)
                //{
                //    list.Add(new UserRoleDTO() { RoleName = item });
                //}
                var model =  mapper.Map<EditUserViewModel>(user);

                model.Roles = userRoles;

                var AllHotelsOfUser=  db.HotelUsers.Where(a => a.UsersId == user.Id).Select(a => a.Hotel.Name).Distinct();

                if(AllHotelsOfUser.Count() > 0)
                {
                    foreach (var hotel in AllHotelsOfUser)
                    {
                        model.Hotels.Add(hotel);
                    }
                }
                

                //var model = new EditUserViewModel
                //{
                //    Id = user.Id,
                //    UserName = user.UserName,
                //    Email = user.Email,
                //    //Claims = userClaims.Select(c => c.Value).ToList(),
                //    Roles = userRoles
                //};

                return model;
            }
            else
            {
                return null;
            }
        }

        //-------------------------------------------------------------------------------
        public async Task<IdentityResult> EditUserPostAsync(EditUserViewModel userModel)
        {
            //var user = await userManager.FindByIdAsync(userModel.Id);
            db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
           var user = db.Users.Where(a => a.Id == userModel.Id).FirstOrDefault();

           
            //user

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            if (userModel.Password != "" && userModel.Password != null)
            {

                var Result11 = await userManager.ResetPasswordAsync(user, token, userModel.Password);


                //await SendConfirmationEmail(user, userModel.Password);
            }

            var Result = await EditUserRoles(userModel.Roles, user);

              await EditUserHotels(userModel.Hotels, user);


            if (user != null)
            {

                /*---------------*/

                
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.Email = userModel.Email;
                user.CNIC = userModel.CNIC;
                user.Address = userModel.Address;
                user.PhoneNumber = userModel.PhoneNumber;
                user.UserName = userModel.Email;

                //string passwordHash;
                //using(var hmac  = new HMACSHA512())
                //{
                //    passwordHash = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userModel.Password)));
                //}
                //user.PasswordHash = passwordHash;

                //var result = await userManager.UpdateAsync(user);
                //var result =   db.Users.Update(user);

                db.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

               await db.SaveChangesAsync();

                return IdentityResult.Success;
                //if (result.Succeeded)
                //{

                //    return result;
                //}
                //else
                //{
                //    return IdentityResult.Failed();
                //}
            }
            else
            {
                return IdentityResult.Failed();
            }


        }


        //--------------------------------------------
        public async Task DeleteUser(string Id)
        {

            var UserObj = await userManager.FindByIdAsync(Id);


            //var result = await userManager.DeleteAsync(UserObj);
            UserObj.IsActive = false;
            db.Entry(UserObj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            await db.SaveChangesAsync();




        }

        public bool UserAlreadyExist(string Email)
        {
            var User_ =  db.Users.Where(a => a.Email == Email && a.IsActive == true).FirstOrDefault();
            if (User_ == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public async Task AddUserAccount(UserViewModel userViewModel)
        {

            var DeactivatedUser = db.Users.Where(a => a.Email == userViewModel.Email && a.IsActive == false).FirstOrDefault();

            if(DeactivatedUser != null)
            {
                DeactivatedUser.FirstName = userViewModel.FirstName;
                DeactivatedUser.LastName = userViewModel.LastName;
                DeactivatedUser.UserName = userViewModel.Email;
                DeactivatedUser.CNIC = userViewModel.CNIC;
                DeactivatedUser.Address = userViewModel.Address;
                DeactivatedUser.PhoneNumber = userViewModel.PhoneNumber  ;
                DeactivatedUser.IsActive = true;


                var Result2 = await EditUserRoles(userViewModel.Roles, DeactivatedUser);

                await EditUserHotels(userViewModel.Hotels, DeactivatedUser);


                db.Entry(DeactivatedUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                await db.SaveChangesAsync();



            }
            else { 

                var user = new User()
                {
                    Email = userViewModel.Email,
                    FirstName = userViewModel.FirstName,
                    LastName = userViewModel.LastName,
                    UserName = userViewModel.Email,
                    CNIC = userViewModel.CNIC,
                    Address = userViewModel.Address,
                    PhoneNumber = userViewModel.PhoneNumber  ,
                    IsActive = true
            };


                //            var UserObj =  mapper.Map<User>(userDTO);
                var Result  = await userManager.CreateAsync(user, userViewModel.Password);

                //var Result = await userManager.CreateAsync(user);

                if (Result.Succeeded)
                {
                     await userManager.AddToRolesAsync(user, userViewModel.Roles);
                }

                if(userViewModel.Hotels.Count > 0)
                {
                    foreach (var hotel in userViewModel.Hotels)
                    {
                        var HotelObj = db.Hotels.Where(a => a.Name == hotel).FirstOrDefault();

                        db.HotelUsers.Add(new IGHportalAPI.Models.HotelUser() { HotelsId = HotelObj.Id, UsersId = user.Id });

                        await db.SaveChangesAsync();
                    }
                }

            }

        }
    }
}
