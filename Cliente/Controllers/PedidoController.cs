using API.Domain.Models;
using API.Infraestructure.Data;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly DataContent _banco;

        public PedidoController(DataContent banco)
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
                var pedidos = await _banco.PedidoDetalhes
                                .Include(x => x.Produto)
                                .ToListAsync();

                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }


        [Authorize]
        [HttpPost]
        [Route("criarNovo")]
        public async Task<IActionResult> Post(int idCliente, double cupom)
        {
            try
            {
                if (idCliente < 1)
                    throw new Exception("Cliente não autenticado.");

                var detalhesPedido = new List<PedidoDetalhes>();
                var itemCarrinhos = await _banco.ItemCarrinho
                                        .Include(x => x.Produto)
                                        .ToListAsync();

                if (itemCarrinhos.Count < 1) return null;

                var cliente = await _banco.Cliente.Where(x => x.Id == idCliente).FirstOrDefaultAsync();

                if (cliente == null) return BadRequest();

                var pedido = new Pedido()
                {
                    Cliente = cliente,
                    DataPedido = DateTime.Now,
                    DataEnvio = DateTime.Now.AddDays(1),
                    Status = EStatus.Realizado,
                    TipoEnvio = ETipoEnvio.Correio
                };

                foreach (var item in itemCarrinhos)
                {
                    detalhesPedido.Add(new PedidoDetalhes()
                    {
                        Produto = item.Produto,
                        Quantidade = item.Quantidade,
                        Subtotal = item.Produto.Preco * item.Quantidade * (1 - cupom),
                        Pedido = pedido
                    });
                }

                await _banco.PedidoDetalhes.AddRangeAsync(detalhesPedido);
                await _banco.SaveChangesAsync();

                _banco.ItemCarrinho.RemoveRange(itemCarrinhos);
                await _banco.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
    }
}
