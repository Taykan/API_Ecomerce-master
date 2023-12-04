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
    public class ProdutoController : ControllerBase
    {
        private readonly DataContent _banco;

        public ProdutoController(DataContent banco)
        {
            _banco = banco;
        }

        [HttpGet]
        [Route("buscarTodos")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var produtos = await _banco.Produto.ToListAsync();

                var listasProduto = new ListasProduto();
                listasProduto.Populares = produtos;
                listasProduto.Promocoes = produtos
                                            .OrderBy(x=>x.Preco)
                                            .ToList();

                return Ok(listasProduto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("buscarPorId")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var produto = await _banco.Produto.FirstOrDefaultAsync(x => x.Id == id);
                return Ok(produto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("criarNovo")]
        public async Task<IActionResult> Post(string nomeProduto,
                                        string descricaoProduto,
                                        string imagem,
                                        double preco)
        {
            try
            {
                var produto = new Produto
                {
                    NomeProduto = nomeProduto,
                    DescricaoProduto = descricaoProduto,
                    Imagem = imagem,
                    Preco = preco
                };

                await _banco.AddAsync(produto);
                await _banco.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("editar")]
        public async Task<IActionResult> Put(int id, [FromBody] Produto produto)
        {
            if (id != produto.Id)
                return BadRequest();

            try
            {
                _banco.Update(produto);
                await _banco.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("apagar")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var produto = await _banco.Produto
                            .FirstOrDefaultAsync(produto => produto.Id == id);

                if (produto == null)
                    return BadRequest();

                _banco.Remove(produto);
                await _banco.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class ListasProduto
    {
        public List<Produto> Populares { get; set; }
        public List<Produto> Promocoes { get; set; }
    }
}
