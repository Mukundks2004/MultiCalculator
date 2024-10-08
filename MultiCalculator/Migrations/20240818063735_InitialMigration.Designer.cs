﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MultiCalculator.Database;

#nullable disable

namespace MultiCalculator.Migrations
{
    [DbContext(typeof(CalculatorDbContext))]
    [Migration("20240818063735_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.7");

            modelBuilder.Entity("MultiCalculator.Database.Models.CalculationHistoryModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("AnswerSteps")
                        .HasColumnType("TEXT");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("QuestionSenderId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("QuestionSenderId");

                    b.ToTable("CalculationHistory");
                });

            modelBuilder.Entity("MultiCalculator.Database.Models.ChatBotHistoryModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("AnswerHistory")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ChatBotUserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("QuestionHistory")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ChatBotUserId");

                    b.ToTable("ChatBotHistory");
                });

            modelBuilder.Entity("MultiCalculator.Database.Models.OpenAiQuestionsModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("QuestionSenderId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("QuestionSenderId");

                    b.ToTable("OpenAiQuestions");
                });

            modelBuilder.Entity("MultiCalculator.Database.Models.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AmountOfGeneratedPdfs")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GeneratedPdfLocations")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("MultiCalculator.Database.Models.CalculationHistoryModel", b =>
                {
                    b.HasOne("MultiCalculator.Database.Models.UserModel", "QuestionSender")
                        .WithMany("calculationHistory")
                        .HasForeignKey("QuestionSenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("QuestionSender");
                });

            modelBuilder.Entity("MultiCalculator.Database.Models.ChatBotHistoryModel", b =>
                {
                    b.HasOne("MultiCalculator.Database.Models.UserModel", "ChatBotUser")
                        .WithMany("chatBotHistory")
                        .HasForeignKey("ChatBotUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChatBotUser");
                });

            modelBuilder.Entity("MultiCalculator.Database.Models.OpenAiQuestionsModel", b =>
                {
                    b.HasOne("MultiCalculator.Database.Models.UserModel", "QuestionSender")
                        .WithMany("openAiQuestions")
                        .HasForeignKey("QuestionSenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("QuestionSender");
                });

            modelBuilder.Entity("MultiCalculator.Database.Models.UserModel", b =>
                {
                    b.Navigation("calculationHistory");

                    b.Navigation("chatBotHistory");

                    b.Navigation("openAiQuestions");
                });
#pragma warning restore 612, 618
        }
    }
}
