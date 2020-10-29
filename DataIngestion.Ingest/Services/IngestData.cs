﻿using System;
using DataIngestion.DB.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DataIngestion.Ingest.Services
{
    public class IngestDataService
    {
        private readonly IDataIngestionRepository _dataIngestionRepository;
        private readonly IConfigurationRoot _configurationRoot;

        public IngestDataService(IDataIngestionRepository dataIngestionRepository, IConfigurationRoot configurationRoot)
        {
            _dataIngestionRepository = dataIngestionRepository ?? throw new ArgumentNullException(nameof(dataIngestionRepository));
            _configurationRoot = configurationRoot ?? throw new ArgumentNullException(nameof(configurationRoot));
        }


    }
}