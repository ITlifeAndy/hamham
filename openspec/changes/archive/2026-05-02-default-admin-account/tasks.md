## 1. Core Implementation

- [x] 1.1 Create a `UserSeeder` class in `HamHam.Infrastructure` to handle default user creation.
- [x] 1.2 Implement the `SeedAsync` method in `UserSeeder` to check for existing users and create the `admin` account with password `hamham`.
- [x] 1.3 Assign the `Admin` role to the created default user.
- [x] 1.4 Integrate `UserSeeder.SeedAsync()` into the `Program.cs` startup sequence in `HamHam.Api`.

## 2. Verification

- [ ] 2.1 Run the system with a clean database and verify the `admin` user is created in the `Users` table.
- [ ] 2.2 Restart the system and verify that no duplicate `admin` account is created.
- [ ] 2.3 Use the Chrome extension to log in with `admin` and `hamham` to verify access.
