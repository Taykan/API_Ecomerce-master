using API.Domain.Models;
using API.Infraestructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly DataContent _banco;

        public ClienteController(DataContent banco)
        {
            _banco = banco;
        }

        [Authorize]
        [HttpGet]
        [Route("buscarTodos")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var clientes = await _banco.Cliente.ToListAsync();

                var clientesDTO = new List<ClienteDTO>();
                foreach (var cliente in clientes)
                {
                    clientesDTO.Add(new()
                    {
                        Id = cliente.Id,
                        NomeCliente = cliente.NomeCliente,
                        Email = cliente.Email,
                        Endereco = cliente.Endereco,
                    });
                }

                return Ok(clientesDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpPost]
        [Route("criarNovo")]
        public async Task<IActionResult> Post([FromBody] Cliente clienteNovo)
        {
            try
            {
                var cliente = new Cliente
                {
                    NomeCliente = clienteNovo.NomeCliente,
                    Email = clienteNovo.Email,
                    Endereco = clienteNovo.Endereco,
                    Senha = clienteNovo.Senha
                };

                cliente.SenhaSetHash();

                await _banco.AddAsync(cliente);
                await _banco.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [Authorize]
        [HttpPut]
        [Route("editar")]
        public async Task<IActionResult> Put(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id)
                return BadRequest();

            try
            {
                _banco.Update(cliente);
                await _banco.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("apagar")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cliente = await _banco.Cliente
                    .FirstOrDefaultAsync(cliente => cliente.Id == id);

                if (cliente != null)
                {
                    _banco.Remove(cliente);
                    await _banco.SaveChangesAsync();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
    }
}
