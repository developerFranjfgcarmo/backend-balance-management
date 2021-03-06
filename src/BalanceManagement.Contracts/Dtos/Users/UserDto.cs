﻿namespace BalanceManagement.Contracts.Dtos.Users
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Nick { get; set; }
        public string PhoneNumber { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public int RoleId { get; set; }
    }
}
