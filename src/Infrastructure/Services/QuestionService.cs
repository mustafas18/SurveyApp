﻿using Core.Dtos;
using Core.Entities;
using Core.IntegrationEvents.Events;
using Core.Interfaces;
using Core.Interfaces.IRepositories;
using Infrastructure.Data.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IRepository<Question> _questionRepository;
        private readonly IQuestionRepository _questionDapper;
        private readonly IRepository<Sheet> _sheetRepository;
        private readonly IMediator _mediator;
        private readonly ISheetRepository _sheetDapperRepository;

        public QuestionService(IRepository<Question> questionRepository,
            IRepository<Sheet> sheetRepository,
            IQuestionRepository questionDapper,
            IMediator mediator,
            ISheetRepository sheetDapperRepository)
        {
            _questionRepository = questionRepository;
            _sheetRepository = sheetRepository;
            _mediator = mediator;
            _sheetDapperRepository = sheetDapperRepository;
            _questionDapper = questionDapper;
        }
        public async Task<string> CreateAsync(string sheetId, Question question)
        {
            question.SheetId = sheetId;
            question.UserId = "";
            question.SheetVersion = _sheetDapperRepository.GetLatestVersion(sheetId);
            question.Order = CountSheetQuestion(sheetId, question.SheetVersion)+1;
            await _sheetDapperRepository.AddQuestion(sheetId,question);
            await _mediator.Publish(new SheetUpdatedEvent(sheetId));
            return "OK";
        }
        public async Task<bool> DeleteQuestionAsync(int questionId)
        {
            var sheetId = await _questionDapper.DeleteAsync(questionId);
            await _mediator.Publish(new SheetUpdatedEvent(sheetId));
            return true;
        }
        public int CountSheetQuestion(string sheetId, int? sheetVersion)
        {
            if (sheetVersion != null)
            {
                return _questionRepository
                      .Where(s => s.SheetId == sheetId && s.SheetVersion == sheetVersion && s.Deleted == false)
                      .Count();
            }
            else
            {
                return _questionRepository
                      .Where(s => s.SheetId == sheetId && s.Deleted == false)
                      .Count();
            }

        }
        public async Task<IEnumerable<QuestionDto>> GetBySheetId(string sheetId, int? sheetVersion)
        {
            if (sheetVersion != null)
            {
                return _questionRepository
                      .Where(s => s.SheetId == sheetId && s.SheetVersion == sheetVersion && s.Deleted==false)
                      .Select(q => new QuestionDto
                      {
                          Id = q.Id,
                          Text = q.Text,
                          Required = q.Required,
                          Type = q.Type,
                          VariableId = q.VariableId,
                          Order = q.Order
                      })
                      .OrderBy(q=>q.Order)
                      .AsEnumerable();
            }
            else
            {
                return _questionRepository
                      .Where(s => s.SheetId == sheetId && s.Deleted == false)
                      .Select(q => new QuestionDto
                      {
                          Id = q.Id,
                          Text = q.Text,
                          Required = q.Required,
                          Type = q.Type,
                          VariableId = q.VariableId,
                          Order = q.Order
                      })
                      .OrderBy(q => q.Order)
                      .AsEnumerable();
            }

        }

        public List<Question> UpdateQuestionOrder(QuestionOrderDto questionDto)
        {
            var latestVersion = _sheetDapperRepository.GetLatestVersion(questionDto.SheetId);
            var sheet = _sheetRepository.Where(s => s.SheetId == questionDto.SheetId && s.Version == latestVersion)
                                        .Include(nameof(Sheet.Questions))
                                        .FirstOrDefault();
            if(sheet?.Questions==null)
            {
                return null;
            }
            List<int> orders = questionDto.Questions.OrderBy(q => q.order).Select(s=>s.Id).ToList();
            var     orderedQuestion = sheet.Questions?.OrderBy(o => orders.IndexOf(o.Id)).ToList();
            int i = 1;
            orderedQuestion.ForEach(q => { q.Order = i++; });
            sheet.SheetQuestions(orderedQuestion);
            _sheetDapperRepository.SaveChanges(sheet);
            _mediator.Publish(new SheetUpdatedEvent(questionDto.SheetId));
            return sheet.Questions.ToList();
        }
    }
}
