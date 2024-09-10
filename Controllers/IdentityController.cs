using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityMS.Data;
using IdentityMS.Models;
using IdentityMS.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace IdentityMS.Controllers;
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly IdentityMsDbContext dbContext;
    private readonly ConnectionFactory rabbitMqConnectionFactory;

    public IdentityController(IdentityMsDbContext dbContext, IOptionsSnapshot<RabbitMqOptions> optionsSnapshot)
    {
        this.dbContext = dbContext;
        this.rabbitMqConnectionFactory = new ConnectionFactory() {
                HostName = optionsSnapshot.Value.HostName,
                UserName = optionsSnapshot.Value.UserName,
                Password = optionsSnapshot.Value.Password
            };
    }

    [HttpPost("api/[controller]/[action]")]
    public async Task<IActionResult> SignUp(User user)
    {
        try
        {
            this.dbContext.Users.Add(user);

            using var connection = this.rabbitMqConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            var destination = "new_user";

            var result = channel.QueueDeclare(
                queue: destination,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            var userJson = JsonSerializer.Serialize(user);

            var messageInBytes = Encoding.UTF8.GetBytes(userJson);

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: destination,
                basicProperties: null,
                body: messageInBytes
            );

            return base.Created();
        }
        catch (Exception ex)
        {
            return base.BadRequest(ex.Message);
        }
    }
}