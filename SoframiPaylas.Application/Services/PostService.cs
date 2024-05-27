using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SoframiPaylas.Application.DTOs.Post;
using SoframiPaylas.Application.Interfaces;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        public PostService(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<string> CreatePostAsync(CreatePostDto postDto)
        {
            var postItem = _mapper.Map<Post>(postDto);

            return await _postRepository.CreatePostAsync(postItem);
        }

        public async Task<bool> DeletePostAsync(string id)
        {
            var postItem = await _postRepository.GetPostByIdAsync(id);
            if (postItem == null)
            {
                throw new Exception("Post not found.");
            }

            return await _postRepository.DeletePostAsync(id);
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync()
        {
            var posts = await _postRepository.GetPostAllAsync();
            return posts.Select(e =>
    {
        var postDto = _mapper.Map<PostDto>(e.post);
        postDto.PostId = e.Id;
        return postDto;
    });
        }

        public async Task<PostDto> GetPostByIdAsync(string id)
        {
            var e = await _postRepository.GetPostByIdAsync(id);
            if (e == null)
            {
                throw new KeyNotFoundException($"No post found with ID {id}");
            }
            var postDto = _mapper.Map<PostDto>(e);
            postDto.PostId = id;
            return postDto;
        }

        public async Task<bool> UpdatePostAsync(string id, UpdatePostDto postDto)
        {
            var postItem = await _postRepository.GetPostByIdAsync(id);
            if (postItem == null)
            {
                throw new Exception("Post not found.");
            }
            _mapper.Map(postDto, postItem);
            return await _postRepository.UpdatePostAsync(id, postItem);
        }
        public async Task<IEnumerable<PostDto>> GetByUserIdPostAllAsync(string userId)
        {
            var posts = await _postRepository.GetByUserIdPostAllAsync(userId);
            return posts.Select(p =>
            {
                var postDto = _mapper.Map<PostDto>(p.post);
                postDto.PostId = p.postId;
                return postDto;
            });
        }

        public async Task<IEnumerable<PostDto>> GetPostsByIdsAsync(List<string> postIds)
        {
            var posts = await _postRepository.GetPostsByIdsAsync(postIds);
            return posts.Select(p =>
            {
                var postDto = _mapper.Map<PostDto>(p.post);
                postDto.PostId = p.postId;
                return postDto;
            });
        }
    }
}

