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
    public class CarrinhoController : ControllerBase
    {
        private readonly DataContent _banco;

        public CarrinhoController(DataContent banco)
        {
            _banco = banco;
        }

        [HttpGet]
        [Route("ver")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var itens = await _banco.ItemCarrinho
                        .Include(x => x.Produto)
                        .ToListAsync();

                return Ok(itens);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpGet]
        [Route("valorTotal")]
        public async Task<IActionResult> GetValorTotal()
        {
            try
            {
                var itens = await _banco.ItemCarrinho
                            .Include(x => x.Produto)
                            .ToListAsync();

                var valorTotal = 0.0;

                foreach (var item in itens)
                {
                    valorTotal += item.Produto.Preco * item.Quantidade;
                }
                return Ok(valorTotal);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpPost]
        [Route("adicionar")]
        public async Task<IActionResult> Post(int idProduto,
                                              int quantidade)
        {
            try
            {
                var produto = await _banco.Produto
                            .FirstOrDefaultAsync(x => x.Id == idProduto);

                var carrinho = new ItemCarrinho
                {
                    DataAdicao = DateTime.Now,
                    Produto = produto,
                    Quantidade = quantidade,
                };

                await _banco.AddAsync(carrinho);
                await _banco.SaveChangesAsync();

                return Ok(carrinho);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpPut]
        [Route("editar")]
        public async Task<IActionResult> Put(int id,
                                      [FromBody] ItemCarrinho carrinho)
        {
            if (id != carrinho.Id)
                return BadRequest();

            try
            {
                _banco.Update(carrinho);
                await _banco.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpDelete]
        [Route("apagar")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var carrinho = await _banco.ItemCarrinho
                        .FirstOrDefaultAsync(carrinho => carrinho.Id == id);

                if (carrinho != null)
                {
                    _banco.Remove(carrinho);
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
