using CampusEats.Api.Data;
using CampusEats.Api.Dtos;
using CampusEats.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Api.Services;

public class MenuService : IMenuService
{
    private readonly AppDbContext _context;

    public MenuService(AppDbContext context)
    {
        _context = context;
    }

    private static MenuItemDto ToDto(MenuItem m) =>
        new(m.Id, m.Name, m.Price, m.Category, m.Available);

    public IEnumerable<MenuItemDto> GetAll()
    {
        return _context.MenuItems
            .AsNoTracking()
            .Select(m => ToDto(m))
            .ToList();
    }

    public MenuItemDto? GetById(int id)
    {
        var item = _context.MenuItems
            .AsNoTracking()
            .FirstOrDefault(m => m.Id == id);

        return item is null ? null : ToDto(item);
    }

    public MenuItemDto Create(CreateMenuItemDto dto)
    {
        var item = new MenuItem
        {
            Name = dto.Name,
            Price = dto.Price,
            Category = dto.Category,
            Available = true
        };

        _context.MenuItems.Add(item);
        _context.SaveChanges();

        return ToDto(item);
    }

    public bool Update(int id, CreateMenuItemDto dto)
    {
        var item = _context.MenuItems
            .FirstOrDefault(m => m.Id == id);

        if (item is null)
            return false;

        item.Name = dto.Name;
        item.Price = dto.Price;
        item.Category = dto.Category;

        _context.SaveChanges();

        return true;
    }

    public bool Delete(int id)
    {
        var item = _context.MenuItems
            .FirstOrDefault(m => m.Id == id);

        if (item is null)
            return false;

        _context.MenuItems.Remove(item);
        _context.SaveChanges();

        return true;
    }
}