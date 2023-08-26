using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StriveSteady.Data;
using StriveSteady.Models;
using System.Security.Cryptography;
using API.Controllers;
using System.Text;

namespace StriveSteady.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly StriveSteadyContext _context;

        public UsersController(StriveSteadyContext context)
        {
            _context = context;
        }

        //encryption function (copy pasted from the internet)
        public static string EncryptString(string plainText, string key, string iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        //Decrypt function (copy pasted from the internet)
        public static string DecryptString(string cipherText, string key, string iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }


        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(User user)
        {
            if(user.FirstName == null || user.FirstName == "" || user.LastName == null || user.LastName == "" || user.Email == null || user.Email == "" || user.Password == null || user.Password == "" || user.PasswordRepeat == null || user.PasswordRepeat == "")
            {
                throw new NullReferenceException("User registration failed: Please fill in all required fields.");
            }

            if(user.Password != user.PasswordRepeat)
            {
                throw new Exception("Password and Password repeat are not identical");
            }

            if(user.Password.Length < 5 )
            {
                throw new Exception("Password not long enough (minimum 5 characters)");
            }

             if(user.FirstName.Length < 3 || user.LastName.Length < 3)
            {
                throw new Exception("First name or last name not long enough (minimum 3 characters)");
            }

            var passwordEnc = EncryptString(user.Password, "0123456789ABCDEF", "FEDCBA9876543210");
            var passwordRepeatEnc = EncryptString(user.PasswordRepeat, "0123456789ABCDEF", "FEDCBA9876543210");
            var userRegister = new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = passwordEnc,
                PasswordRepeat = passwordRepeatEnc
            };

            

            _context.User.Add(userRegister);
            await _context.SaveChangesAsync();

            return userRegister;
        }

        // GET: Users/Details/5
        [HttpGet("GetUserById")]
        public async Task<User> GetUserById(int? id)
        {
            if (id == null || _context.User == null)
            {
                throw new Exception("User with id "+ id +  " does not exist");
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                throw new Exception("User with id"+ id +  "does not exist");
            }

            return user;
        }

        public async Task<User> GetUserByMail(string mail)
        {

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Email == mail);
            
            if (user == null)
            {
                throw new Exception("User with mail " +mail + " does not exist");
            }

            return user;
        }

        [HttpGet("LogIn")]
        public async Task<User> LogIn(string email, string password)
        {
            var user = await GetUserByMail(email);

            //decrypting password
            var passwordDecrypt = DecryptString(user.Password, "0123456789ABCDEF", "FEDCBA9876543210");

            //validation password
            if (passwordDecrypt != password)
            {
                throw new Exception("Incorrect password, try again");
            }

            return user;
            
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return Ok(user);
        }

        [HttpDelete("Users/delete/{id}")]
        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            _context.SaveChanges();

            return Ok(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'StriveSteadyContext.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
