using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using scheduler_desktop_app.Database;
using scheduler_desktop_app.Exceptions;
using scheduler_desktop_app.Models;

namespace scheduler_desktop_app.Data
{
    internal class MySqlCustomerRepository : ICustomerRepository
    {
        private const string AuditUser = "test";
        private const int DefaultCityId = 1;
        private const string DefaultPostalCode = "00000";

        public List<Customer> GetAll()
        {
            try
            {
                EnsureConn();

                var customers = new List<Customer>();

                using (var cmd = DBConnection.Conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT c.customerId, c.customerName, c.active,
                               a.address, a.phone
                        FROM customer c
                        JOIN address a ON c.addressId = a.addressId
                        ORDER BY c.customerId;";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Customer
                            {
                                CustomerId = Convert.ToInt32(reader["customerId"]),
                                CustomerName = reader["customerName"].ToString(),
                                Address = reader["address"].ToString(),
                                Phone = reader["phone"].ToString(),
                                Active = Convert.ToInt32(reader["active"]) == 1
                            });
                        }
                    }
                }

                return customers;
            }
            catch (Exception ex)
            {
                throw new CustomerOperationException("GetAll", "Unable to load customers.", ex);
            }
        }

        public void Add(Customer customer)
        {
            try
            {
                EnsureConn();
                if (customer == null) throw new ArgumentNullException(nameof(customer));

                var now = DateTime.UtcNow;

                using (var tx = DBConnection.Conn.BeginTransaction())
                {
                    try
                    {
                        int addressId = InsertAddress(tx, customer.Address, customer.Phone, now);

                        using (var cmd = DBConnection.Conn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandText = @"
                                INSERT INTO customer
                                    (customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy)
                                VALUES
                                    (@name, @addressId, @active, @createDate, @createdBy, @lastUpdate, @lastUpdateBy);";

                            cmd.Parameters.AddWithValue("@name", customer.CustomerName);
                            cmd.Parameters.AddWithValue("@addressId", addressId);
                            cmd.Parameters.AddWithValue("@active", customer.Active ? 1 : 0);
                            cmd.Parameters.AddWithValue("@createDate", now);
                            cmd.Parameters.AddWithValue("@createdBy", AuditUser);
                            cmd.Parameters.AddWithValue("@lastUpdate", now);
                            cmd.Parameters.AddWithValue("@lastUpdateBy", AuditUser);

                            cmd.ExecuteNonQuery();
                        }

                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomerOperationException("Add", "Unable to add customer record.", ex);
            }
        }

        public void Update(Customer customer)
        {
            try
            {
                EnsureConn();
                if (customer == null) throw new ArgumentNullException(nameof(customer));

                var now = DateTime.UtcNow;

                using (var tx = DBConnection.Conn.BeginTransaction())
                {
                    try
                    {
                        int addressId = GetAddressIdForCustomer(customer.CustomerId, tx);

                        using (var cmd = DBConnection.Conn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandText = @"
                                UPDATE address
                                SET address = @address,
                                    phone = @phone,
                                    lastUpdate = @lastUpdate,
                                    lastUpdateBy = @lastUpdateBy
                                WHERE addressId = @addressId;";

                            cmd.Parameters.AddWithValue("@address", customer.Address);
                            cmd.Parameters.AddWithValue("@phone", customer.Phone);
                            cmd.Parameters.AddWithValue("@lastUpdate", now);
                            cmd.Parameters.AddWithValue("@lastUpdateBy", AuditUser);
                            cmd.Parameters.AddWithValue("@addressId", addressId);

                            cmd.ExecuteNonQuery();
                        }

                        using (var cmd = DBConnection.Conn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandText = @"
                                UPDATE customer
                                SET customerName = @name,
                                    active = @active,
                                    lastUpdate = @lastUpdate,
                                    lastUpdateBy = @lastUpdateBy
                                WHERE customerId = @customerId;";

                            cmd.Parameters.AddWithValue("@name", customer.CustomerName);
                            cmd.Parameters.AddWithValue("@active", customer.Active ? 1 : 0);
                            cmd.Parameters.AddWithValue("@lastUpdate", now);
                            cmd.Parameters.AddWithValue("@lastUpdateBy", AuditUser);
                            cmd.Parameters.AddWithValue("@customerId", customer.CustomerId);

                            cmd.ExecuteNonQuery();
                        }

                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomerOperationException("Update", "Unable to update customer record.", ex);
            }
        }

        public void Delete(int customerID)
        {
            try
            {
                EnsureConn();

                using (var tx = DBConnection.Conn.BeginTransaction())
                {
                    try
                    {
                        int addressId = GetAddressIdForCustomer(customerID, tx);

                        using (var cmd = DBConnection.Conn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandText = "DELETE FROM appointment WHERE customerId = @customerId;";
                            cmd.Parameters.AddWithValue("@customerId", customerID);
                            cmd.ExecuteNonQuery();
                        }

                        using (var cmd = DBConnection.Conn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandText = "DELETE FROM customer WHERE customerId = @customerId;";
                            cmd.Parameters.AddWithValue("@customerId", customerID);
                            cmd.ExecuteNonQuery();
                        }

                        using (var cmd = DBConnection.Conn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandText = "DELETE FROM address WHERE addressId = @addressId;";
                            cmd.Parameters.AddWithValue("@addressId", addressId);
                            cmd.ExecuteNonQuery();
                        }

                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomerOperationException("Delete", "Unable to delete customer record.", ex);
            }
        }

        private static void EnsureConn()
        {
            if (DBConnection.Conn == null || DBConnection.Conn.State != System.Data.ConnectionState.Open)
                throw new InvalidOperationException("Database connection is not open. Call DBConnection.startConnection() at startup.");
        }

        private static int InsertAddress(MySqlTransaction tx, string address, string phone, DateTime now)
        {
            using (var cmd = DBConnection.Conn.CreateCommand())
            {
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    INSERT INTO address
                        (address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy)
                    VALUES
                        (@address, @address2, @cityId, @postalCode, @phone, @createDate, @createdBy, @lastUpdate, @lastUpdateBy);
                    SELECT LAST_INSERT_ID();";

                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@address2", "");
                cmd.Parameters.AddWithValue("@cityId", DefaultCityId);
                cmd.Parameters.AddWithValue("@postalCode", DefaultPostalCode);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@createDate", now);
                cmd.Parameters.AddWithValue("@createdBy", AuditUser);
                cmd.Parameters.AddWithValue("@lastUpdate", now);
                cmd.Parameters.AddWithValue("@lastUpdateBy", AuditUser);

                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        private static int GetAddressIdForCustomer(int customerId, MySqlTransaction tx)
        {
            using (var cmd = DBConnection.Conn.CreateCommand())
            {
                cmd.Transaction = tx;
                cmd.CommandText = "SELECT addressId FROM customer WHERE customerId = @customerId;";
                cmd.Parameters.AddWithValue("@customerId", customerId);

                object result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                    throw new InvalidOperationException("Customer record not found.");

                return Convert.ToInt32(result);
            }
        }
    }
}